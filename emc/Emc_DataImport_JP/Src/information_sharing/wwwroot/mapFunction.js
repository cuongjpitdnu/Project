/* -------------------------------------変数-------------------------------------------- */
var map = null;						// 地図描画用の変数
var mapView = null;					// 地図の表示設定用変数
var drawInteraction = null;			// 図形描画操作インタラクション
var modifyInteraction = null;		// 図形描画編集インタラクション
var selectInteraction = null;		// 図形描画選択インタラクション

var mapUrl = null;					// 地図機能のサーバ処理呼び出し用URL
var kixMapLayerTile = null;			// 関空全体地図のタイルレイヤ
var locationLayer = null;			// 場所表示用図形レイヤ
var locationViewInfo = null;		// 場所表示時の描画設定情報
var locationStyle = null;			// 場所表示時のスタイル
var highlightViewInfo = null;		// 場所ハイライト時の描画設定情報
var highlightStyle = null;			// 場所ハイライト時のスタイル
var viewLocationId = [];			// 場所表示で指定した図形ID

var mapMode = 0;					// 現在の地図表示モード（0：モードなし、1:場所検索、2：場所登録）
var drawMode = 0;					// 現在の図形描画モード（0：モードなし、1:点、2：面、3:円、4：線、9：クリア）
var beforeClearDrawMode = -1;		// クリアモード切替前の図形描画モード
var drawFeatureType = 0;			// 地図描画中の図形種別（0：描画なし、1:点、2：面、3:円、4：線）

/* -------------------------------------定数-------------------------------------------- */
var projOpenLayers = "EPSG:3857";	// OpenLayersの座標系
var projPostGis = "EPSG:4326";		// PostGisの座標系
var mapFormat = "image/png";		// 地図画像のフォーマット
var wmsVersion = "1.3.0";			// WMSバージョン

/* -----------------------------------初期値設定----------------------------------------- */
var min_zoom  = 10;					// ズームの最小値
var max_zoom  = 19;					// ズームの最大値
var init_zoom = 14;					// ズームの初期値
var init_angle = 39.1;				// 回転角度の初期値
var init_center_lon = 135.233341;	// 中心の経度
var init_center_lat = 34.432884;	// 中心の緯度

/* ------------------------------------地図機能------------------------------------------ */

var mapSite = "map";

/**
* 地図メイン画面（プロトタイプ）
*/
function initMap(url) {
	// 地図機能のサーバ処理呼び出し用URLを設定
	if (url) {
		mapUrl = this.joinTrailingSlash(url);
	} else {
		// 引数が未指定の場合は固定値を使用
		mapUrl = "http://localhost:8080/";
	}

	// アプリケーションの設定ファイルより各設定情報を取得
	$.ajax({
		type: "POST",
		url: mapUrl + mapSite + "/jsp/initMap.jsp",
		dataType: "json",
		async: false	// 同期処理
	}).done(function(data, textStatus, jqXHR) {
		// レスポンスデータから各情報を取り出す
		locationViewInfo = data["location_view_info"];
		highlightViewInfo = data["highlight_view_info"];
	}).fail(function(jqXHR, textStatus, errorThrown) {
		console.log(errorThrown);
	});

	// 関西空港MAPのタイルLayerを生成
	kixMapLayerTile = new ol.layer.Tile({
		name: "関西空港MAP",
		source: new ol.source.XYZ({
			url: mapUrl + mapSite + "/files/kix_map_tiles/{z}/{x}/{y}.png",
			// CORS対応
			crossOrigin: "anonymous"
		}),
		isBaseLayer: true
	});

	// 地図の表示設定オブジェクトを生成
	mapView = new ol.View({
		projection: projOpenLayers,
		zoom: init_zoom,
		maxZoom: max_zoom,
		minZoom: min_zoom
	});

	// 地図オブジェクトを生成
	map = new ol.Map({
		target: 'map',
		view: mapView
	});

	// 関西空港の全体地図を追加
	map.addLayer(kixMapLayerTile);

	// 表示縮尺のスライダーバー追加
	map.addControl(new ol.control.ZoomSlider());

	// 表示縮尺追加
	map.addControl(new ol.control.ScaleLine());

	// 地図の回転設定追加
	var dragRotatemInteraction = new ol.interaction.DragRotate({
		condition: ol.events.condition.platformModifierKeyOnly
	});
	map.addInteraction(dragRotatemInteraction);

	// 場所表示図形のスタイルオブジェクトを生成
	var locationStrokeWidth = locationViewInfo["location_stroke_width"];
	var locationStrokeColor = locationViewInfo["location_stroke_color"];
	var locationFillColor = locationViewInfo["location_fill_color"];
	var locationImageFileName = locationViewInfo["location_image_file_name"];
	var locationImageAnchor = locationViewInfo["location_image_anchor"].split(',');
	locationStyle = new ol.style.Style({
		fill: new ol.style.Fill({color: locationFillColor}),	// 塗りつぶし
		stroke: new ol.style.Stroke({							// 枠線
			color: locationStrokeColor,
			width: parseInt(locationStrokeWidth)}),
		image: new ol.style.Icon({								// ピン（anchorで位置調整）
			src: mapUrl + mapSite + "/files/" + locationImageFileName,
			anchor: [parseFloat(locationImageAnchor[0]), parseFloat(locationImageAnchor[1])]})
	});

	// 場所ハイライト図形のスタイルオブジェクトを生成
	var highlightStrokeWidth = highlightViewInfo["highlight_stroke_width"];
	var highlightStrokeColor = highlightViewInfo["highlight_stroke_color"];
	var highlightFillColor = highlightViewInfo["highlight_fill_color"];
	var highlightImageFileName = highlightViewInfo["highlight_image_file_name"];
	var highlightImageAnchor = highlightViewInfo["highlight_image_anchor"].split(',');
	highlightStyle = new ol.style.Style({
		fill: new ol.style.Fill({color: highlightFillColor}),	// 塗りつぶし
		stroke: new ol.style.Stroke({							// 枠線
			color: highlightStrokeColor,
			width: parseInt(highlightStrokeWidth)}),
		image: new ol.style.Icon({								// ピン（anchorで位置調整）
			src: mapUrl + mapSite + "/files/" + highlightImageFileName,
			anchor: [parseFloat(highlightImageAnchor[0]), parseFloat(highlightImageAnchor[1])]})
	});

	// 場所表示用図形レイヤオブジェクトを生成
	locationLayer = new ol.layer.Vector({
		source: new ol.source.Vector(),
		style: locationStyle
	});
	locationLayer.setMap(map);

	// 初期設定値で地図表示
	var coordinate = [init_center_lon, init_center_lat];
	this.viewMap(coordinate, init_zoom, init_angle);
}

/**
* 地図表示
* @param coordinate 中心座標
* @param zoomLevel 表示縮尺
* @param angle 回転角度
*/
function viewMap(coordinate, zoomLevel, angle) {

	// 地図中心座標の指定
	if (coordinate) mapView.setCenter(ol.proj.transform(coordinate, projPostGis, projOpenLayers));

	// 表示縮尺の設定
	if (zoomLevel) mapView.setZoom(zoomLevel);

	// 回転角度の設定
	if (angle) mapView.setRotation((parseFloat(angle) * Math.PI/180));
}

/**
* 場所表示
* @param locationIdArray 場所ID（配列）
*/
function viewLocation(locationIdArray) {

	// 場所レイヤ表示
	$.ajax({
		type: "POST",
		url: mapUrl + mapSite + "/jsp/viewLocation.jsp",
		data: {
			locationIdArray: locationIdArray
		},
		dataType: "json",
		traditional: true,	// リクエストパラメータに配列を含む
		async: false		// 同期処理
	}).done(function(data, textStatus, jqXHR) {

		// 取得したデータからレスポンス値を取り出す
		var responseJSON = data["responseJSON"];

		// 表示対象の場所情報より図形オブジェクトを作成し、場所レイヤに表示設定する
		var features = [];
		var viewLocationInfo = responseJSON.viewLocationInfo;
		for (var i=0; i < viewLocationInfo.length; i++) {
			createFeature(features, viewLocationInfo[i]);
		}
		locationLayer.setSource(new ol.source.Vector({features: features}));

	}).fail(function(jqXHR, textStatus, errorThrown) {
		console.log(errorThrown);
	});
}

/**
* 場所検索
* @param coordinate 座標
* @return 検索結果 座標近辺の場所IDの配列（座標から近い順）
*/
function searchLocation(coordinate) {
	var locationIdArray = [];

	// 場所検索
	$.ajax({
		type: "POST",
		url: mapUrl + mapSite + "/jsp/searchLocation.jsp",
		data: {
			coordinate: coordinate,
			zoomLevel: Math.floor(mapView.getZoom())
		},
		dataType: "json",
		traditional: true,	// リクエストパラメータに配列を含む
		async: false		// 同期処理
	}).done(function(data, textStatus, jqXHR) {
		// 取得したデータからレスポンス値を取り出す
		var responseJSON = data["responseJSON"];
		// レスポンス値から検索結果を取り出す
		locationIdArray = responseJSON["locationIdArray"];

	}).fail(function(jqXHR, textStatus, errorThrown) {
		console.log(errorThrown);
	});

	return locationIdArray;
}

/**
* 場所登録
* @param locationId 場所ID
* @param featureType 図形種別 1：点 2：面 3：円 4：線
* @param geometryInfo ジオメトリ情報
* @return 実行結果 0：正常終了 100：異常終了
*/
function registLocation(locationId, featureType, geometryInfo) {
	var result = 100;

	// 場所登録
	$.ajax({
		type: "POST",
		url: mapUrl + mapSite + "/jsp/registLocation.jsp",
		data: {
			locationId: locationId,
			featureType: featureType,
			geometryInfo: geometryInfo
		},
		dataType: "json",
		traditional: true,	// リクエストパラメータに配列を含む
		async: false		// 同期処理
	}).done(function(data, textStatus, jqXHR) {
		// 正常終了
		result = 0;

	}).fail(function(jqXHR, textStatus, errorThrown) {
		console.log(errorThrown);
	});

	return result;
}

/**
* 場所削除
* @param locationId 場所ID
* @return 実行結果 0：正常終了 100：異常終了
*/
function deleteLocation(locationId) {
	var result = 100;

	// 場所削除
	$.ajax({
		type: "POST",
		url: mapUrl + mapSite + "/jsp/deleteLocation.jsp",
		data: {
			locationId: locationId
		},
		dataType: "json",
		async: false		// 同期処理
	}).done(function(data, textStatus, jqXHR) {
		// 正常終了
		result = 0;

	}).fail(function(jqXHR, textStatus, errorThrown) {
		console.log(errorThrown);
	});

	return result;
}

/**
* 場所ハイライト
* @param locationId 場所ID
* @return 実行結果 0：正常終了 100：異常終了
*/
function highlightLocation(locationId) {
	var result = 100;

	try {
		// 引数なしの場合はすべてハイライトする
		if (!locationId) locationId = -1;

		// 数値チェック
		var intId = parseInt(locationId, 10);
		if (isNaN(intId)) {throw new Error('場所ハイライトエラー： 場所IDは数値を指定してください。')};

		// インシデント発生箇所の場所図形情報をすべて取得
		var features = locationLayer.getSource().getFeatures();

		// 対象の場所IDに該当する図形をハイライト表示する
		for (var i=0; i < features.length; i++) {
			var feature = features[i];
			if (intId === -1) {
				feature.setStyle(highlightStyle);
				continue;
			}
			if (!feature.attributes) continue;
			if (feature.attributes.locationId === intId) {
				feature.setStyle(highlightStyle);
			}
		}
		// 正常終了
		result = 0;
	} catch (e) {
		console.log(e.message);
	}

	return result;
}

/**
* ハイライト解除
* @param locationId 場所ID
* @return 実行結果 0：正常終了 100：異常終了
*/
function clearHighlight(locationId) {
	var result = 100;

	try {
		// 引数なしの場合はすべてハイライト解除する
		if (!locationId) locationId = -1;

		// 数値チェック
		var intId = parseInt(locationId, 10);
		if (isNaN(intId)) {throw new Error('ハイライト解除エラー： 場所IDは数値を指定してください。')};

		// インシデント発生箇所の場所図形情報をすべて取得
		var features = locationLayer.getSource().getFeatures();

		// 対象の場所IDに該当する図形のハイライトを解除する
		for (var i=0; i < features.length; i++) {
			var feature = features[i];
			if (intId === -1) {
				feature.setStyle(locationStyle);
				continue;
			}
			if (!feature.attributes) continue;
			if (feature.attributes.locationId === intId) {
				feature.setStyle(locationStyle);
			}
		}
		// 正常終了
		result = 0;
	} catch(e) {
		console.log(e.message);
	}

	return result;
}


/* -----------------------------------ローカル関数----------------------------------------- */
/**
* 文字列のURL対応
* @param str 対象文字列
* @return 末尾に「/」を付与した文字列
*/
function joinTrailingSlash(str) {
	// 文字列の末尾が「/」でない場合は「/」を付与
	var pattern = /\/$/;
	return pattern.test(str) ? str : str + '/';
}

/**
* 図形オブジェクト作成
* @param features 図形格納配列
* @param locationInfo 図形情報
*/
function createFeature(features, locationInfo) {

	// 図形種別応じた図形を作成
	var wkt = locationInfo.wkt.match(/^GEOMETRYCOLLECTION\((.+)\)$/)[1];
	switch (locationInfo.feature_type) {
		case 1:		// 点
			var geomArray = wkt.split(',');
			for (var i=0; i < geomArray.length; i++) {
				// 点座標から図形を作成
				var point = geomArray[i].match(/^POINT\((.+)\)$/)[1];
				var geomPoint = new ol.geom.Point(point.split(' '));
				var feature = new ol.Feature(geomPoint.transform(projPostGis, projOpenLayers));
				feature.attributes = {locationId: locationInfo.location_id};
				features.push(feature);
			}
			break;
		case 2:		// 面
			var geomArray = wkt.split(')),');
			for (var i=0; i < geomArray.length; i++) {
				// 面座標から図形を作成
				var polygonCoord = [];
				var geomCoord = [];
				var strPoints = geomArray[i].match(/^POLYGON\(\((.+)/)[1];
				// 最後の文字が「))」の場合は削除
				if (strPoints.slice(-2) == '))') strPoints = strPoints.slice(0, -2);
				var pointArray = strPoints.split(',');
				for (var j=0; j < pointArray.length; j++) {
					var point = pointArray[j];
					geomCoord.push(point.split(' '));
				}
				polygonCoord.push(geomCoord);
				var geomPolygon = new ol.geom.Polygon(polygonCoord);
				var feature = new ol.Feature(geomPolygon.transform(projPostGis, projOpenLayers));
				feature.attributes = {locationId: locationInfo.location_id};
				features.push(feature);
			}
			break;
		case 3:		// 円
			var pointArray = locationInfo.center_coord.split(',');
			var radiusArray = locationInfo.radius.split(',');
			for (var i=0; i < pointArray.length; i++) {
				// 中心座標と半径から円図形を作成
				var center_coord = pointArray[i];
				var radius = radiusArray[i];
				var geomCircle = new ol.geom.Circle(center_coord.split(' '));
				var feature = new ol.Feature(geomCircle.transform(projPostGis, projOpenLayers));
				feature.getGeometry().setRadius(parseFloat(radius));
				feature.attributes = {locationId: locationInfo.location_id};
				features.push(feature);
			}
			break;
		case 4:		// 線
			var geomArray = wkt.split('),');
			for (var i=0; i < geomArray.length; i++) {
				// 線座標から図形を作成
				var geomCoord = [];
				var strPoints = geomArray[i].match(/^LINESTRING\((.+)/)[1];
				// 最後の文字が「)」の場合は削除
				if (strPoints.slice(-1) == ')') strPoints = strPoints.slice(0, -1);
				var pointArray = strPoints.split(',');
				for (var j=0; j < pointArray.length; j++) {
					var point = pointArray[j];
					geomCoord.push(point.split(' '));
				}
				var geomLineString = new ol.geom.LineString(geomCoord);
				var feature = new ol.Feature(geomLineString.transform(projPostGis, projOpenLayers));
				feature.attributes = {locationId: locationInfo.location_id};
				features.push(feature);
			}
			break;
	}
}


/* -----------------------------------サンプル処理----------------------------------------- */
/**
* 地図表示位置初期化
*/
function initMapPosition() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";

	var coordinate = [init_center_lon, init_center_lat];
	this.viewMap(coordinate, init_zoom, init_angle);
}

/**
* 地図表示モード初期化
*/
function initMapMode() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";
	$("#lblMapMode")[0].innerHTML = "モードなし";

	// 地図表示モードを切替
	this.changeMapMode(0);
}

/**
* 場所表示呼び出し
*/
function callViewLocation() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";

	// 場所IDの入力値を取得
	var locationId = $('#txtLocationId').val();

	// 場所表示処理実行
	var locationIdArray = locationId.split(',');
	this.viewLocation(locationIdArray);

	// 表示中の場所IDを保持
	viewLocationId = locationIdArray;

	// 図形描画モードを保持
	this.changeDrawMode(drawMode);
}

/**
* 場所ハイライト呼び出し
*/
function callHighlightLocation() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";

	// 場所IDの入力値を取得
	var locationId = $('#txtLocationId').val();

	// 場所ハイライト処理実行
	var result = this.highlightLocation(locationId);

	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "場所ハイライト 実行結果:" + result;
}

/**
* ハイライト解除呼び出し
*/
function callClearHighlight() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";

	// 場所IDの入力値を取得
	var locationId = $('#txtLocationId').val();

	// ハイライト解除処理実行
	var result = this.clearHighlight(locationId);

	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "ハイライト解除 実行結果:" + result;
}

/**
* 場所検索モード切替
*/
function changeSearchLocationMode() {
	// TODO 処理結果確認用コード
	// $("#outputText")[0].innerHTML = "";
	// $("#lblMapMode")[0].innerHTML = "場所検索";

	// 地図表示モードを切替
	this.changeMapMode(1);

	// 場所検索操作中のシングルクリックイベントを設定
	map.on("singleclick", searchLocationClick);
}
/* 場所検索操作中のシングルクリックイベント */
function searchLocationClick(evt) {
	// TODO 処理結果確認用コード
	// $("#outputText")[0].innerHTML = "";
	clearHighlight();

	// 地図上のクリック座標を取得し、場所検索実行
	var coordinate = ol.proj.transform(evt.coordinate, projOpenLayers, projPostGis);
	var locationIdArray = searchLocation(coordinate);

	// TODO 処理結果確認用コード
	// $("#outputText")[0].innerHTML = "場所検索結果：" + locationIdArray;
	// 表示中の場所IDが存在していた場合ハイライト
	
	if(locationIdArray.length > 0){
		getBull(locationIdArray);
	}
	var strIdsList = JSON.stringify(locationIdArray);
	localStorage.setItem("cookie", strIdsList);
	
	for (var i=0; i < locationIdArray.length; i++) {
		highlightLocation(locationIdArray[i]);
		
		/*
		if (viewLocationId.indexOf(String(locationIdArray[i])) >= 0) {
			highlightLocation(locationIdArray[i]);
		}
		*/
	}
}

/**
 * ハイライトを削除
 * @param
 * @return
 */
function clearHighlightWhenClose(){
	var arrIds = JSON.parse(localStorage.getItem("cookie"));
	clearHighlight();
	localStorage.removeItem("cookie");
}

/**
 * モーダルで開いている掲示のリスト
 * @param locationId 場所ID
 * @return モーダルで開いている掲示のリスト
 */
function getBull(locationIdArray) {
	var csrf = $('#csrf').val();
	var data = [
		{name: 'X-CSRF-TOKEN',value: csrf },
		{name: 'action',value: 'load_bull_map' },
		{name: 'arrLocationIds',value: locationIdArray },
	];
	
	$('#modal-body').html('');
	
	// 場所検索
	$.ajax({
		url:"portal_proc.php",
		type : "POST",
		dataType:"text",
		data : data,
		success : function (result){
			if(result.length > 2){
				$('#myModal').modal({
					backdrop: 'static'
				});
				$('#modal-body').html(result);
			}else{
				$('#myModal').modal('hide');
			}
		}
	});
}

/**
* 場所登録モード切替
*/
function changeRegistLocationMode() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";
	$("#lblMapMode")[0].innerHTML = "場所登録";

	// 地図表示モードを切替
	this.changeMapMode(2);

	// 描画モードの選択値を取得して図形描画モード切替
	var radioVal = $("input[name='drawMode']:checked").val();
	this.changeDrawMode(parseInt(radioVal));
}

/**
* 場所登録呼び出し
*/
function callRegistLocation() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";

	// 描画中の図形情報を取得
	var features = locationLayer.getSource().getFeatures();

	// 図形種別に応じてジオメトリ情報を取得する
	var geometryInfo = [];
	switch (drawFeatureType) {
		case 1:		// 点（ピン）
			// 描画されている点（ピン）の個数で配列生成
			for (var i=0; i < features.length; i++) {
				var coordinates = features[i].getGeometry().getCoordinates();
				geometryInfo.push(ol.proj.transform(coordinates, projOpenLayers, projPostGis));
			}
			break;
		case 2:		// 面(枠囲み)
			// 描画されている面(枠囲み)の個数で配列生成
			for (var i=0; i < features.length; i++) {
				var coordinates = features[i].getGeometry().getCoordinates();
				// 面(枠囲み)毎の頂点の個数で配列生成
				var coordPoints = [];
				for (var j=0; j < coordinates[0].length; j++) {
					coordPoints.push(ol.proj.transform(coordinates[0][j], projOpenLayers, projPostGis));
				}
				geometryInfo.push(coordPoints);
			}
			break;
		case 3:		// 円
			// 描画されている円の個数で配列生成
			for (var i=0; i < features.length; i++) {
				var geometry = features[i].getGeometry();
				// 中心座標と半径で配列生成
				var circleInfo = [];
				circleInfo.push(ol.proj.transform(geometry.getCenter(), projOpenLayers, projPostGis));
				circleInfo.push(geometry.getRadius());
				geometryInfo.push(circleInfo);
			}
			break;
		case 4:		// 線
			// 描画されている線の個数で配列生成
			for (var i=0; i < features.length; i++) {
				var coordinates = features[i].getGeometry().getCoordinates();
				// 線毎の頂点の個数で配列生成
				var coordPoints = [];
				for (var j=0; j < coordinates.length; j++) {
					coordPoints.push(ol.proj.transform(coordinates[j], projOpenLayers, projPostGis));
				}
				geometryInfo.push(coordPoints);
			}
			break;
		default:	// 上記以外の場合は処理終了
			return;
	}

	// 場所IDの入力値を取得
	var locationId = $('#txtLocationId').val();

	// 場所登録処理実行
	var result = this.registLocation(locationId, drawFeatureType, geometryInfo);

	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "場所登録 実行結果:" + result;
}

/**
* 場所削除呼び出し
*/
function callDeleteLocation() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";

	// 場所IDの入力値を取得
	var locationId = $('#txtLocationId').val();

	// 場所削除処理実行
	var result = this.deleteLocation(locationId);
	$("#outputText")[0].innerHTML = "場所削除 実行結果:" + result;
}

/**
* 図形全クリア
*/
function locationLayerClearAll() {
	// TODO 処理結果確認用コード
	$("#outputText")[0].innerHTML = "";

	// 表示中の場所IDを初期化
	viewLocationId = [];

	// 表示中の場所レイヤをすべてクリア
	locationLayer.getSource().clear();
}

/**
* 地図表示モード切替
*/
function changeMapMode(mode) {
	// モード切替なしの場合、処理終了
	if (mapMode === mode) return;

	// 現在の地図表示モードに応じて初期化
	switch (mapMode) {
		case 0:		// モードなし
			break;
		case 1:		// 場所検索
			// 場所検索モードを解除
			map.un("singleclick", searchLocationClick);
			break;
		case 2:		// 場所登録
			// 図形描画モードを解除
			this.changeDrawMode(0);
			drawFeatureType = 0;
			break;
	}

	// 切替後の図形描画モードに応じて初期化
	switch (mode) {
		case 0:		// モードなし
		case 1:		// 場所検索
			// 図形描画用機能を非活性
			this.registObjDisabled(true);
			break;
		case 2:		// 場所登録
			// 図形描画用機能を活性化
			this.registObjDisabled(false);
			break;
	}

	// 地図表示モード変更
	mapMode = mode;
}
/* 図形描画用機能の活性状態制御 */
function registObjDisabled(isDisabled) {
	// 場所登録確定と描画モード切替の活性状態を切替
	$("#btnRegistLocation").prop('disabled', isDisabled);
	$("#chkDrawPoint").prop('disabled', isDisabled);
	$("#chkDrawLinestring").prop('disabled', isDisabled);
	$("#chkDrawPolygon").prop('disabled', isDisabled);
	$("#chkDrawCircle").prop('disabled', isDisabled);
	$("#chkDrawClear").prop('disabled', isDisabled);
}

/**
* 図形描画モード切替
*/
function changeDrawMode(mode) {

	// 現在の図形描画モードに応じて初期化
	switch (drawMode) {
		case 0:		// モードなし
			break;
		case 1:		// 点
		case 2:		// 面
		case 3:		// 円
		case 4:		// 線
			map.removeInteraction(drawInteraction);
			map.removeInteraction(modifyInteraction);
			break;
		case 9:		// クリア
			map.removeInteraction(selectInteraction);
			break;
	}

	// モード切替なしまたはクリアモードの切替時は場合は図形をクリアしない
	if (!(drawMode === mode || mode === beforeClearDrawMode || mode === 9)) {
		this.locationLayerClearAll();
	}

	// 切替後の図形描画モードに応じて切替
	this.initDrawMode(mode);

	// 図形描画モード変更
	drawMode = mode;
}
/* 図形描画モード初期化 */
function initDrawMode(mode) {
	switch (mode) {
		case 0:		// モードなし
			break;
		case 1:		// 点
			beforeClearDrawMode = -1;
			callDrawPoint();
			break;
		case 2:		// 面
			beforeClearDrawMode = -1;
			callDrawPolygon();
			break;
		case 3:		// 円
			beforeClearDrawMode = -1;
			callDrawCircle();
			break;
		case 4:		// 線
			beforeClearDrawMode = -1;
			callDrawLineString();
			break;
		case 9:		// クリア
			beforeClearDrawMode = drawMode;
			callDrawClear();
			break;
	}
}
/* 点（ピン）描画モード切替 */
function callDrawPoint() {
	// 描画中の図形種別を設定（Point:1）
	drawFeatureType = 1;

	// GeometryType：Pointで図形描画・編集インタラクション追加
	addDrawInteraction("Point");
	addModifyInteraction();
}
/* 面(枠囲み)描画モード切替 */
function callDrawPolygon() {
	// 描画中の図形種別を設定（Polygon:2）
	drawFeatureType = 2;

	// GeometryType：Polygonで図形描画・編集インタラクション追加
	addDrawInteraction("Polygon");
	addModifyInteraction();
}
/* 円描画モード切替 */
function callDrawCircle() {
	// 描画中の図形種別を設定（Circle:3）
	drawFeatureType = 3;

	// GeometryType：Circleで図形描画・編集インタラクション追加
	addDrawInteraction("Circle");
	addModifyInteraction();
}
/* 線描画モード切替 */
function callDrawLineString() {
	// 描画中の図形種別を設定（LineString:4）
	drawFeatureType = 4;

	// GeometryType：LineStringで図形描画・編集インタラクション追加
	addDrawInteraction("LineString");
	addModifyInteraction();
}
/* クリアモード切替 */
function callDrawClear() {
	// 図形選択インタラクション追加
	addSelectInteraction();
}

/**
* OpenLayers描画操作設定処理
*/
/* 図形描画インタラクション */
function addDrawInteraction(geometryType) {
	// 図形描画インタラクションを生成し、地図に追加
	drawInteraction = new ol.interaction.Draw({
		source: locationLayer.getSource(),
		type: geometryType
	});
	map.addInteraction(drawInteraction);
}
/* 図形編集インタラクション */
function addModifyInteraction() {
	// 図形編集インタラクションを生成し、地図に追加
	modifyInteraction = new ol.interaction.Modify({
		source: locationLayer.getSource()
	});
	map.addInteraction(modifyInteraction);
}
/* 図形選択インタラクション */
function addSelectInteraction() {
	// 図形編集インタラクションを生成し、地図に追加
	selectInteraction = new ol.interaction.Select({
		source: locationLayer.getSource()
	});
	map.addInteraction(selectInteraction);
	selectInteraction.on("select", deleteFeature, this);
}
/* 図形選択イベント */
function deleteFeature(evt) {
	// 選択した図形を削除
	var feature = evt.selected[0];
	locationLayer.getSource().removeFeature(feature);
	selectInteraction.getFeatures().remove(feature);
}
/* キーイベント制御 */
document.onkeydown = function(evt) {
	var keyCode = false;
	if (evt) {
		if (evt.keyCode) {
			keyCode = evt.keyCode;
		} else if (evt.which) {
			keyCode = evt.which;
		}
	}

	/* Escキー押下時、描画中の図形をクリア */
	if(keyCode==27){
		try {drawInteraction.setActive(false)} catch(e){};
		try {drawInteraction.setActive(true)} catch(e){};
	}
}

function mapRefresh() {
	// 背景地図を再描画する
	this.map.updateSize();
}

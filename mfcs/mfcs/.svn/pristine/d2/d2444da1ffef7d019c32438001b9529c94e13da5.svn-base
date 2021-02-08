function fncJsonParse(json) {
	var rst = [];
	try { rst = JSON.parse(html.decode(json.replace(/\\/g, '\\\\')
											.replace(/&amp;quot;/g, '\\"')
											.replace(/&quot;/g, '"')
											.replace(/&amp;/g, '&')
											.replace(/&lt;/g, '<')
											.replace(/&gt;/g, '>')
											.replace(/&apos;/g, '\''))); } catch (err) { }
	return rst;
}

function convertHTML(str) {
	htmlEntities = {
		'&':'&amp;',
		'<':'&lt;',
		'>':'&gt;',
		'"':'&quot;',
		'\'':"&apos;"
	};
	return str.split('')
				.map(entity => htmlEntities[entity] || entity).join('')
				.replace(/&amp;nbsp;/g, '&nbsp;')
				.replace(/&amp;/g, '&')
				.replace(/&#039;/g, '\'');
}

function escapeCharEntities() {
	var map = {
		"&": "&amp;",
		"<": "&lt;",
		">": "&gt;",
		"\"": "&quot;",
		"\\": "&bsol;",
		"'": "&apos;"
	};
	return map;
}

var mapkeys = '', mapvalues = '';
var html = {
	encodeRex : function () {
		return  new RegExp(mapkeys, 'g'); // "[&<>"']"
	},
	decodeRex : function () {
		return  new RegExp(mapvalues, 'g'); // "(&amp;|&lt;|&gt;|&quot;|&apos;)"
	},
	encodeMap : JSON.parse( JSON.stringify( escapeCharEntities () ) ), // json = {&: "&amp;", <: "&lt;", >: "&gt;", ": "&quot;", ': "&apos;"}
	decodeMap : JSON.parse( JSON.stringify( swapJsonKeyValues( escapeCharEntities () ) ) ),
	encode : function ( str ) {
		var encodeRexs = html.encodeRex();
		return str.replace(encodeRexs, function(m) { return html.encodeMap[m]; }); // m = < " > SpecialChars
	},
	decode : function ( str ) {
		var decodeRexs = html.decodeRex();
		return str.replace(decodeRexs, function(m) { return html.decodeMap[m]; }); // m = &lt; &quot; &gt;
	}
};

function swapJsonKeyValues ( json ) {
	var count = Object.keys( json ).length;
	var obj = {};
	var keys = '[', val = '(', keysCount = 1;
	for(var key in json) {
		if ( json.hasOwnProperty( key ) ) {
			obj[ json[ key ] ] = key;
			keys += key;
			if( keysCount < count ) {
				val += json[ key ]+'|';
			} else {
				val += json[ key ];
			}
			keysCount++;
		}
	}
	keys += ']';    val  += ')';
	mapkeys = keys;
	mapvalues = val;
	return obj;
}

$(function() {
	$('ul.pagination li.page-item').each(function(i, e) {
		const valueHref = $(this).find('a.page-link').attr('href');
		if (typeof valueHref !== 'undefined') {
			const arrSplit = valueHref.split('&err1=');
			$(this).find('a.page-link').attr('href', arrSplit[0]);
		}
	});
});
VM環境にて以下の手順で設定を行ってください。
①C:\phpフォルダにcurlフォルダを作成する。
②作成したcurlフォルダに、「cacert.pem」ファイルを設置する。
③php.iniファイルを開き、下記の設定を変更する。
	;curl.cainfo = 
	
	↓
	
	curl.cainfo = 'C:\php\curl\cacert.pem'

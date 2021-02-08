<?php

define ('OTP_OK',                       0                       );  //业务成功响应

define ('OTP_PARAM_TYPE_ERR',           1                       );  //业务请求参数缺少或参数类型不正确
define ('OTP_DEAL_REQ_FAILED',          2                       );  //处理请求失败，未知错误原因
define ('OTP_DEAL_DATA_ERR',            3                       );  //处理数据解析错误，无效的请求
define ('OTP_REQ_NOT_SUPPORT',          4                       );  //请求不支持，无效的请求
define ('OTP_REQ_INVALID',              5                       );  //无效的客户端请求
define ('OTP_DATA_DECRPT_FAILED',       6                       );  //数据解密失败，请检查安全密钥是否填写错误或已被服务器端更新

define ('OTP_APP_ID_INVALID',           101                     );  //无效应用标识，请检查是否已经添加应用或应用标识是否正确
define ('OTP_ACCESSTOKEN_INVALID',      102                     );  //无效的会话标识
define ('OTP_ACCESSTOKEN_EXPIRED',      103                     );  //会话标识已过期
define ('OTP_URI_INVALID',              104                     );  //无效的回调URI

define ('OTP_USER_NOT_EXIST',           201                     );  //用户不存在，请检查您输入的用户帐号是否正确
define ('OTP_USER_DISABLED',            202                     );  //用户未启用
define ('OTP_USER_LOCKED',              203                     );  //用户已锁定，请稍后重试
define ('OTP_USER_NO_TOKEN',            204                     );  //用户未绑定令牌
define ('OTP_USER_BIND_TOKEN_FAILED',   205                     );  //用户绑定令牌失败
define ('OTP_USER_NOT_HAVE_PERMISSION', 206                     );  //用户没有应用的访问权限
define ('OTP_USER_PHONE_NUBER_EXSIT',   207                     );  //手机号已存在
define ('OTP_USER_PHONE_TOKEN_FAILED',  208                     );  //手机令牌激活码生成失败
define ('OTP_USER_EXSIT',               209                     );  //用户已存在
define ('OTP_USER_NO_FACEE_TOKNE',      210                     );  //用户未激活脸谱令牌
define ('OTP_USER_NO_MOBILE_TOKNE',     211                     );  //用户未激活手机令牌
define ('OTP_USER_NO_WECHAT_TOKNE',     212                     );  //用户未激活微信令牌
define ('OTP_USER_NO_VOICE_TOKNE',      213                     );  //用户未激活语音令牌
define ('OTP_USER_NO_GESTURE_TOKNE',    214                     );  //用户未激活手势令牌

define ('OTP_TOKEN_NOT_EXIST',          301                     );  //令牌不存在
define ('OTP_TOKEN_DISABLED',           302                     );  //令牌未启用
define ('OTP_TOKEN_LOCKED',             303                     );  //令牌已锁定，请稍后重试
define ('OTP_TOKEN_IS_BINDED',          304                     );  //令牌已经被绑定
define ('OTP_TOKEN_NOT_OWNED',          305                     );  //该企业下不存在该令牌
define ('OTP_TOKEN_DECRPT_FAILED',      306                     );  //解密令牌数据失败
define ('OTP_TOKEN_EXPIRED',            307                     );  //令牌已过期

define ('OTP_INVALID',                  401                     );  //动态口令验证失败
define ('OTP_HAVE_USED',                402                     );  //动态口令已经被使用过，请输入新的动态口令
define ('OTP_TOKEN_NEED_SYNC',          403                     );  //云信令时间需要校准
define ('OTP_AUTH_POLICY_INVALID',      404                     );  //无效用户/用户组认证策略
define ('OTP_AUTH_POLICY_ACCEPT',       405                     );  //认证策略为：不进行动态口令认证，直接返回成功
define ('OTP_AUTH_POLICY_REJECT',       406                     );  //认证策略为：不进行动态口令认证，直接返回失败
define ('OTP_TOKEN_SYNC_FAILED',        407                     );  //云信令时间校准失败
define ('OTP_AUTH_POLICY_REJECT_WIFI',  408                     );  //WIFI应用不能使用不进行动态口令认证，直接返回成功的认证策略
define ('OTP_WIFI_SECRET_EMPTY',        409                     );  //公网WIFI密钥为空
define ('OTP_APP_TYPE_INVALID',         410                     );  //应用集成类型与代理类型不匹配，请检查应用类型选择是否正确
define ('OTP_TWO_FACTOR_AUTH',          411                     );  //认证策略为：进行动态口令认证
define ('OTP_PUSH_AUTH_REJECT',         412                     );  //推送认证请求选择了拒绝的处理方式
define ('OTP_AUTH_TIMEOUT',             413                     );  //推送认证请求超时，请重新进行推送
define ('OTP_PUSH_AUTH_NO_TOKEN',       414                     );  //未激活手机云信令或微信云信令，不能进行推送认证
define ('OTP_PUSH_AUTH_NO_TERMINAL',    415                     );  //没有可推送的终端，请检查您的手机云信令是否已经开启
define ('OTP_SCAN_AUTH_FAILED',         416                     );  //扫码认证失败
define ('OTP_USER_MATCH_FAILED',        417                     );  //用户账号不匹配
define ('OTP_REQID_EXPIERD_FAILED',     418                     );  //扫码认证请求已过期
define ('OTP_FACE_AUTH_FAILED',         419                     );  //人脸认证失败
define ('OTP_PUSH_CERT_NOT_EXIST',      420                     );  //没有推送证书
define ('OTP_NO_TOKEN_TYPE_TO_SYNC',    421                     );  //没有可同步的令牌类型
define ('OTP_PUSHAUTH_PROCESS',         422                     );  //认证处理中
define ('OTP_APP_PARAM_INVALID',        423                     );  //云信令客户端参数错误
define ('OTP_CLIENT_TOKEN_DELETED',     424                     );  //客户端令牌己删除
define ('OTP_FACE_DETECT_FAILED',       425                     );  //人脸认证活体检测失败
define ('OTP_POLICY_GRUOP_INVALID',     426                     );  //无效认证策略，用户选择使用组策略，但不属于任何组
define ('OTP_DEVICE_NOT_SUPPORT',       427                     );  //不支持此设备类型
define ('OTP_VOICE_TOKEN_DISABLED',     428                     );  //未启用语音令
define ('OTP_VOICE_AUTH_FAILED',        429                     );  //语音认证失败
define ('OTP_GESTURE_TOKEN_DISABLED',   430                     );  //未启用手势令
define ('OTP_GESTURE_AUTH_FAILED',      431                     );  //手势认证失败
define ('OTP_U2F_ADDR_NOT_MATCH',       432                     );  //U2F受保护地址不区配
define ('OTP_PSUH_TOO_MUCH',            433                     );  //推送太频繁，请稍后重试
define ('FACE_ENGINE_NOT_MATCHED',      434                     );  //人脸引擎类型不匹配
define ('FACE_SERVICE_NOT_MATCHED',     435                     );  //人脸服务类型不匹配
define ('FACE_REGIST_FAILED',           436                     );  //人脸注册失败
define ('OTP_PSUH_USER_NOT_MATCHED',    437                     );  //推送和响应用户不匹配
define ('FACE_SERVICE_CONN_FAILED',     438                     );  //无法连接脸谱服务器
define ('FACE_SERVICE_PROCESS_FAILED',  439                     );  //脸谱服务供应商处理失败
define ('PSUH_SERVICE_CONN_FAILED',     440                     );  //无法连接推送服务器
define ('COMPANY_LOCKED',               441                     );  //企业已锁定
define ('APP_LOCKED',                   442                     );  //应用已锁定
define ('USER_NUM_UPTO_MAX',            443                     );  //用户数已达上限
define ('ACCOUNT_INSUFFICIENT',         444                     );  //企业账号余额不足

define ('OTP_USER_ADD_FAILED',          504                     );  //添加用户信息失败
define ('OTP_PHONE_ADD_FAILED',         505                     );  //添加手机号失败
define ('OTP_PHONE_NOT_EXIST',          506                     );  //手机号不存在

define ('OTP_SEND_SMS_FAILED',          601                     );  //发送短信验证码失败，请重新发送！
define ('OTP_SEND_SMS_TOO_MUCH',        602                     );  //发送短信验证码失败，您的操作过于频繁！

define ('OTP_PARAM_INVALID',            1001                    );  //SDK客户端参数不对
define ('OTP_NOT_INIT',                 1002                    );  //SDK客户端未初始化
define ('OTP_CONF_NOT_EXIST',           1003                    );  //配置文件不存在
define ('OTP_CONF_READ_ERR',            1004                    );  //读取配置文件失败
define ('OTP_DATA_HASH_ERR',            1005                    );  //SDK客户端解密数据失败
define ('OTP_JSON_PROCESS_FAILED',      1006                    );  //SDK客户端JSON处理失败
define ('OTP_INVALID_QR_CODE_FORMAT',   1007                    );  //无效二维码格式
define ('OTP_INVALID_STATE',            1008                    );  //状态检查不一致

define ('OTP_HTTP_TIMEOUT',             1051                    );  //SDK客户端请求超时
define ('OTP_HTTP_CANTCONNECT',         1052                    );  //SDK客户端不能连接
define ('OTP_HTTP_FAIL',                1053                    );  //SDK客户端HTTP请求异常


?>
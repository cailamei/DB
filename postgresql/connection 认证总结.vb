postgresql
1. connection 管理
   1> 	关于postgresql.conf(postgresql.conf 配置监听，服务端口，配置加密方式)
		listen_addresses = 'localhost' 改为 listen_address = '*'
		password_encryption = md5或者scram-sha-256(默认的加密方式)
		知识点1：scram-sha-256加密，它可以用于scram-sha-256、password和MD5经验证，而md5进行加密后，那么它只能认证password和MD5 的认证方式；   
	2>	hba.conf 配置链接认证
		认证过程没有fall_through 或者backup，如果选择了一条记录且认证失败，那么不再考虑后面的记录，所以范围小的应放于范围大的之前才起作用
		如下：		
		host all clm 10.67.36.31/22 md5
        host all all 10.67.36.31/22 trust
		用户 clm 是需要密码认证的 ，并不会因为下边的trust 影响；
		而顺序相反后，则clm 不需要密码认证；
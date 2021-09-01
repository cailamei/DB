ES 学习笔记

es 搭建过程所报错
	[1]: [unknown] uncaught exception in thread [main]org.elasticsearch.bootstrap.StartupException: java.lang.RuntimeException: can not run elasticsearch as root
	[2]: max file descriptors [4096] for elasticsearch process is too low, increase to at least [65536]
	[3]: max virtual memory areas vm.max_map_count [65530] is too low, increase to at least [262144]
	[4]: ERROR: [1] bootstrap checks failed ,system call filters failed to install; check the logs and fix your configuration or disable system call filters at your own risk
     原因：Centos6不支持SecComp，而 默认bootstrap.system_call_filter为true进行检测，所以导致检测失败，失败后直接导致ES不能启动。
	 添加参数bootstrap.system_call_filter: false 即可解决

1. 下载es 资源
   curl -x http://h7108579:pqhkr99ctw@10.36.6.66:3128 -o elasticsearch7.10.2.tar.gz https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-7.10.2-linux-x86_64.tar.gz
   
2. 准备环境
	IP               hostname       nodes             os_version
	-----------      --------       --------------    -------------
	10.67.36.30      ora            master & data      centos 7.7
	10.67.36.31      ora-test       master & data      centos 7.7
	10.67.36.32      ora6           master & data      centos 6.9

3. 三台server卸载swap
   swapoff -a
   fstab 挂在的swap 注释掉
4. 三台server 配置资源限制(文件描述符，线程数，虚拟内存)
	cat >>/etc/security/limits.conf << EOF
	*  soft  nofile 65536
	*  hard  nofile  131072
	*  soft  nproc   4096
	*  hard  nproc   4096
	EOF
	注: 查看/etc/security/limits.d/20-nproc.conf，若soft nproc 为1024，则修改参数值为4096，此文件修改后需要重新登录用户，才会生效
	
	修改 /etc/sysctl.conf ，修改完成后使用sysctl -p 使该参数生效
	cat >>/etc/sysctl.conf <<EOF
	vm.max_map_count=655360 -- 该参数对可用虚拟内存区域个数做限制
	EOF
	

5. 三台server 分别安装JDK
   使用java -version 提示版本信息即可
   
6. 创建启动es 的用户
   es 不可以使用root 用户启动，需建立专门的用户
   useradd elasticsearch
   echo 123456 |passwd --stdin elasticsearch
   
7. 生成证书以及生成p12秘钥，并拷贝p12秘钥文件到其他两台server 的相同路劲下
   ./elasticsearch-certutil ca --生成证书
   ./elasticsearch-certutil cert --ca elastic-stack-ca.p12 --生成p12秘钥
     
8. 修改ES 配置文件
	/root/es01/config/elasticsearch.yml 配置如下：
	
	cluster.name: clm_es_test
	node.master: true
	path.data: /data/es01/data
	path.logs: /data/es01/logs
	bootstrap.memory_lock: true
	network.host: 10.67.36.31
	http.port: 9200
	transport.tcp.port: 9300
	discovery.seed_hosts: ["10.67.36.30:9300","10.67.36.32:9300"]
	cluster.initial_master_nodes: ["es01","es02","es03"]
	gateway.recover_after_nodes: 1
	action.destructive_requires_name: true
	http.cors.enabled: true
	http.cors.allow-origin: "*"
	http.allow-headers: Authorization,X-Requested-With,Content-Type,Content-Length
	xpack.security.enabled: true
	xpack.security.authc.accept_default_password: true
	xpack.security.transport.ssl.enabled: true
	xpack.security.transport.ssl.verification_mode: certificate
	xpack.security.transport.ssl.keystore.path: /home/dbadmin/es03/config/certificates/elastic-certificates.p12
	xpack.security.transport.ssl.truststore.path: /home/dbadmin/es03/config/certificates/elastic-certificates.p12
	
	注：  centos 6 和centos 7 参数有区别，cantos 6 需要添加参数 bootstrap.system_call_filter: false

9. 启动es,其他两台server 也使用相同命令启动
   ./elasticsearch -d --后台方式
10. 设定es 账号密码，依次填入密码即可
   ./bin/elasticsearch-setup-passwords interactive

   
--至此es 集群搭建完成
	
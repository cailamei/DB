	es 参数文件参数解释

		cluster.name: es-cluster #指定es集群名，配置的集群名称，默认是elasticsearch，es服务会通过广播方式自动连接在同一网段下的es服务，通过多播方式进行通信，同一网段下可以有多个集群，通过集群名称这个属性来区分不同的集群
		node.name: xxxx #指定当前es节点名，当前配置所在机器的节点名，你不设置就默认随机指定一个name列表中名字，该name列表在es的jar包中config文件夹里name.txt文件中，其中有很多作者添加的有趣名字
		node.data: false #非数据节点，指定该节点是否存储索引数据，默认为true。
		node.master: false #非master节点，指定该节点是否有资格被选举成为node（注意这里只是设置成有资格， 不代表该node一定就是master），默认是true，es是默认集群中的第一台机器为master，如果这台机挂了就会重新选举master。
		node.attr.rack: r1 #自定义的属性,这是官方文档中自带的，比集群更大得概念，指定节点得部落属性
		bootstrap.memory_lock: true #开启启动es时锁定内存,默认为false,会导致硬盘频繁读，IOPS变高,锁定物理内存地址后，防止es内存被交换出去，也就是避免es使用swap交换分区，频繁的交换，会导致IOPS变高
		network.host: 172.17.0.5 #当前节点的ip地址
		http.port: 9200 #设置当前节点占用的端口号，默认9200
		discovery.seed_hosts: ["172.17.0.3:9300","172.17.0.4:9300","172.17.0.2:9300"] #启动当前es节点时会去这个ip列表中去发现其他节点，此处不需配置自己节点的ip,这里支持ip和ip:port形式,不加端口号默认使用ip:9300去发现节点
		cluster.initial_master_nodes: ["node-1", "node-2", "node-3"] #可作为master节点初始的节点名称,tribe-node不在此列
		gateway.recover_after_nodes: 2 #设置集群中N个节点启动时进行数据恢复，默认为1。可选
		path.data: /path/to/path  #数据保存目录
		path.logs: /path/to/path #日志保存目录
		transport.tcp.port: 9300 #设置集群节点发现的端口
		index.number_of_shards: 5 设置默认索引分片个数，默认为5片。es7 参数文件里该参数无效，默认是1
		index.number_of_replicas: 1设置默认索引副本个数，默认为1个副本。如果采用默认设置，而你集群只配置了一台机器，那么集群的健康度为yellow，也就是所有的数据都是可用的，但是某些复制没有被分配
		path.conf: /path/to/conf 设置配置文件的存储路径，默认是es根目录下的config文件夹。
		path.data: /path/to/data 设置索引数据的存储路径，默认是es根目录下的data文件夹，可以设置多个存储路径，用逗号隔开，例：path.data: /path/to/data1,/path/to/data2
		path.work: /path/to/work 设置临时文件的存储路径，默认是es根目录下的work文件夹。es7 该参数不可用
		path.plugins: /path/to/plugins 设置插件的存放路径，默认是es根目录下的plugins文件夹, 插件在es里面普遍使用，用来增强原系统核心功能。
		bootstrap.mlockall: true 设置为true来锁住内存不进行swapping。因为当jvm开始swapping时es的效率 会降低，所以要保证它不swap，可以把ES_MIN_MEM和ES_MAX_MEM两个环境变量设置成同一个值，并且保证机器有足够的内存分配给es。 同时也要允许elasticsearch的进程可以锁住内# # 存，linux下启动es之前可以通过`ulimit -l unlimited`命令设置。
		network.bind_host: 192.168.0.1设置绑定的ip地址，可以是ipv4或ipv6的，默认为0.0.0.0，绑定这台机器的任何一个ip。
		network.publish_host: 192.168.0.1 设置其它节点和该节点交互的ip地址，如果不设置它会自动判断，值必须是个真实的ip地址。
		network.host: 192.168.0.1 这个参数是用来同时设置bind_host和publish_host上面两个参数。
		transport.tcp.port: 9300设置节点之间交互的tcp端口，默认是9300。
		transport.tcp.compress: true设置是否压缩tcp传输时的数据，默认为false，不压缩。
		http.port: 9200 设置对外服务的http端口，默认为9200。
		http.max_content_length: 100mb 设置内容的最大容量，默认100mb
		http.enabled: false 是否使用http协议对外提供服务，默认为true，开启。
		gateway.type: local gateway的类型，默认为local即为本地文件系统，可以设置为本地文件系统，分布式文件系统，hadoop的HDFS，和amazon的s3服务器等。
		gateway.recover_after_nodes: 1 设置集群中N个节点启动时进行数据恢复，默认为1。
		gateway.recover_after_time: 5m 设置初始化数据恢复进程的超时时间，默认是5分钟。
		gateway.expected_nodes: 2 设置这个集群中节点的数量，默认为2，一旦这N个节点启动，就会立即进行数据恢复。
		cluster.routing.allocation.node_initial_primaries_recoveries: 4 初始化数据恢复时，并发恢复线程的个数，默认为4。
		cluster.routing.allocation.node_concurrent_recoveries: 2 # 添加删除节点或负载均衡时并发恢复线程的个数，默认为4。
		indices.recovery.max_size_per_sec: 0 设置数据恢复时限制的带宽，如入100mb，默认为0，即无限制。
		indices.recovery.concurrent_streams: 5 设置这个参数来限制从其它分片恢复数据时最大同时打开并发流的个数，默认为5。
		discovery.zen.minimum_master_nodes: 1# 设置这个参数来保证集群中的节点可以知道其它N个有master资格的节点。默认为1，对于大的集群来说，可以设置大一点的值（2-4）
		discovery.zen.ping.timeout: 3s 设置集群中自动发现其它节点时ping连接超时时间，默认为3秒，对于比较差的网络环境可以高点的值来防止自动发现时出错。
		discovery.zen.ping.multicast.enabled: false# 设置是否打开多播发现节点，默认是true。
		discovery.zen.ping.unicast.hosts: ["host1", "host2:port", "host3[portX-portY]"]# 设置集群中master节点的初始列表，可以通过这些节点来自动发现新加入集群的节点。
		action.destructive_requires_name: false禁止使用通配符或_all删除索引， 必须使用名称或别名才能删除该索引。
		elasticsearch配置文件中http.cors.x字段用途和用法 cors解释：Cross Origin Resource Sharing 跨域资源共享 
		http.cors.enabled 是否支持跨域，默认为false 
		http.cors.allow-origin 当设置允许跨域，默认为*,表示支持所有域名，如果我们只是允许某些网站能访问，那么可以使用正则表达式。比如只允许本地地址。 /https?:\ /\ /localhost(:[0-9]+)?/ http.cors.max-age 浏览器发送一个“预检”OPTIONS请求，以确定CORS设置。最大年龄定义多久的结果应该缓存。默认为1728000（20天） 
		http.cors.allow-methods 允许跨域的请求方式，默认OPTIONS,HEAD,GET,POST,PUT,DELETE 
		http.cors.allow-headers 跨域允许设置的头信息，默认为X-Requested-With,Content-Type,Content-Length 
		http.cors.allow-credentials 是否返回设置的跨域Access-Control-Allow-Credentials头，如果设置为true,那么会返回给客户端。
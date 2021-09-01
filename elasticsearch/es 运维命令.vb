es 运维命令

报错处理
	1. 创建带有settings 的索引
		{"error":"Content-Type header [application/x-www-form-urlencoded] is not supported","status":406}
		原因: 这个是由于缺少了json数据提交header的声明
		解决方案: curl 后添加  -H "Content-Type: application/json"
		eg:
		curl -H "Content-Type: application/json" -XPUT 'localhost:9200/megacorp/employee/1' -d '{
			"first_name":"John",
			"last_name":"Smith",
			"age":25,
			"about":"I love to go rock climbing",
			"interests":[
				"sports",
				"music"
			]
		}'
1. es 索引相关操作命令
	Elasticsearch的index类似于关系型数据库的库的概念，在保存数据前，要先创建索引  使用curl命令创建  创建一个新的索引，并设置分片数和副本数  创建一个twitter的索引, 设置为3个分片，2个副本，默认5个分片，1个副本 
	curl -XPUT http://localhost:9200/twitter -d'
	{
		"settings" : {
			"index" : {
				"number_of_shards" : 3, 
				"number_of_replicas" : 2 
			}
		}
	}'
	创建一个新的索引test，设置分片数为1，通过mapping初始化一个type1,type1有一个属性field1 
	curl -XPUT http://localhost:9200/test -d'
	{
		"settings" : {
			"number_of_shards" : 1
		},
		"mappings" : {
			"type1" : {
				"properties" : {
					"field1" : { "type" : "text" }
				}
			}
		}
	}'
	创建索引twitter，并添加类型tweet 
	curl -XPUT 'http://localhost:9200/twitter/' -d '
	{
	  "mappings": {
		"tweet": {
		  "properties": {
			"message": {
			  "type": "string"
			}
		  }
		}
	  }
	}'
	添加一个type到存在的索引 
	curl -XPUT  'http://localhost:9200/twitter/_mapping/user' -d '
	{
		  "properties": {
			"username": {
			  "type":"string"
			}
		  }
	}'
	添加一个新的field到存在的type中(在twitter这个type中添加一个field use_name 
	curl -XPUT  'http://localhost:9200/twitter/_mapping/user' -d '
	{
		  "properties": {
			"address": {
			  "type":"string"
			}
		  }
	}'
	如果已经存在index,或者type, field，再次添加同样的type和field将会报错，不能更新  可以更新fileld的情况  type的属性是对象，这种情况可以更新对象 
	curl -XPUT  'http://localhost:9200/my_index -d'
	{
	  "mappings": {
		"user": {
		  "properties": {
			"name": {
			  "properties": {
				"first": {
				  "type": "text"
				}
			  }
			},
			"user_id": {
			  "type": "keyword"
			}
		  }
		}
	  }
	}'
	上面的代码创建了my_index索引，并添加了一个user type, user的属性为对象name, name有两个属性first,user_id，这种情况就可以更新对象的属性 
	curl -XPUT  'http://localhost:9200/my_index/_mapping/user -d'
	{
	  "properties": {
		"name": {
		  "properties": {
			"last": { 
			  "type": "text"
			}
		  }
		},
		"user_id": {
		  "type": "keyword",
		  "ignore_above": 100 
		}
	  }
	}'

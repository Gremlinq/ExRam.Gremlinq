FROM tinkerpop/gremlin-server 
 
RUN sed -i "s/gremlin.tinkergraph.vertexIdManager=LONG/gremlin.tinkergraph.vertexIdManager=UUID/g" /opt/gremlin-server/conf/tinkergraph-empty.properties
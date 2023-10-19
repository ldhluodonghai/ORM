
using Model.Entitys.Dto;
using MySqlX.XDevAPI;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Dao.ESModel;

namespace UserManagement.ServiceDev.ES
{
    public  class PeopleService
    {
        private readonly ElasticClient elasticClient;

        /*var connectionSettings = new ConnectionSettings(pool, (builtin, settings) =>
               new JsonNetSerializer(builtin, settings, () =>
                  new JsonSerializerSettings
                  {
                      TypeNameHandling = TypeNameHandling.All,
                      NullValueHandling = NullValueHandling.Include
                  }
                  )).DefaultIndex(DefaultIndex);*/
        public PeopleService( )
        {           
            
                var node = new Uri("http://localhost:9200");
                var settings = new ConnectionSettings(node);
                settings.DefaultIndex("peoples");
                elasticClient = new ElasticClient(settings);
            
        }

        public IndexResponse Create(People people)
        {
            IndexResponse indexResponse = elasticClient.Index(people, idx => idx.Index("peoples"));
            return indexResponse;
        }
        public IndexResponse Create1(People people)
        {
            var indexResponse = elasticClient.IndexDocument(people);
            return indexResponse;
        }
        public void Create(List<People> peoples)
        {
            elasticClient.IndexMany(peoples,("peoples"));
        }
        public void Delete(People people)
        {
            elasticClient.Delete<People>(people.Id, idx => idx.Index("peoples"));
        }
        public People GetById(string id)
        {
            return elasticClient.Get<People>(id,idx=>idx.Index("peoples")).Source;
        }
        public IEnumerable<People> GetAll()
        {
            //直接查询
            #region
            {
                SearchRequest request = new SearchRequest("peoples");
                ISearchResponse<People> dems = elasticClient.Search<People>(request);
                return dems.Documents;
            }
            #endregion
            #region
            //委托查询
            {
                return elasticClient.Search<People>(s => s.Index("peoples")).Documents;
            }
            #endregion
        }

        public IEnumerable<People> GetPeoplesKeySearch(PeopleDto peopleDto)
        {
            return elasticClient.Search<People>(s => s
            .Index("peoples")
            .Query(q => q
                .Match(mq => mq.Field(f => f.PeopleName).Query(peopleDto.PeopleName))
                )
            ).Documents;
        }
    }

}

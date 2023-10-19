using Service;
using UserManagement.Db;
using Usermanger.Event.Model;

namespace Usermanger.EventDomain.Service
{
    public class EventService:ORMSqlHelper<Events>
    {
        public int AddEvent(Events e)
        {
            int v = Add(e);
            return v;
        }
    }
}

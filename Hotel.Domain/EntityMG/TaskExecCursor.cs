using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.EntityMG
{

    /// <summary>
    /// 任务执行光标
    /// </summary>
    public class TaskExecCursor
    {
        public ObjectId Id { get; set; }

        /// <summary>
        /// 取数记录Id
        /// </summary>
        public long TakeId { get; set; }

        /// <summary>
        /// 执行Id
        /// </summary>
        public long RunId { get; set; }
    }
}

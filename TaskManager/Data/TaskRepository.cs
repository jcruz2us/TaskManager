using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using TaskManager.Models;

namespace TaskManager.Data
{
    /// <summary>
    /// The FlatFileTaskRepository will store results into a text file in the json format
    /// If necessary, the repository will take care of creating the text file. Otherwise, 
    /// it will load the json and store the results into an in memory list of tasks.
    /// 
    /// Everytime there is a change, the undelying file will be updated too.
    /// </summary>
    public class FlatFileTaskRepository : IRepository<Task>
    {
        //the file path of the document where the tasks will be stored
        private const String FILE_PATH = "~/App_Data/tasks.json";
        private String _serverPath;
        private DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(List<Task>));
        private List<Task> _tasks = new List<Task>();

        public FlatFileTaskRepository()
        {
            _serverPath = HttpContext.Current.Server.MapPath(FILE_PATH);
            //load the tasks file only if it exists
            if (File.Exists(_serverPath))
            {
                //wrap in a using block to dispose of the resources
                //and to avoid problems opening/saving the file later on
                using (var stream = File.OpenRead(_serverPath))
                {
                    _tasks = (List<Task>)_serializer.ReadObject(stream);
                }
            }
        }

        public Task Get(long id)
        {
            return _tasks.SingleOrDefault(task => task.Id == id);
        }

        public IList<Task> GetAll()
        {
            return _tasks;
        }

        public void Add(Task entity)
        {
            _tasks.Add(entity);
            Commit();
        }

        public void Remove(Task entity)
        {
            _tasks.Remove(entity);
            Commit();
        }

        public void Update(Task entity)
        {
            var ent = Get(entity.Id);
            ent.Description = entity.Description;
            ent.IsCompleted = entity.IsCompleted;
            Commit();
        }

        private void Commit()
        {
            //wrap in a using block to dispose of the resources
            //and to avoid problems opening/saving the file later on
            using (var stream = File.Create(_serverPath))
            {
                _serializer.WriteObject(stream, _tasks);
            }
        }
    }
}
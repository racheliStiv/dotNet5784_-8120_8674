namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

internal class TaskImplementation : ITask
{
    //CRUD of task
    public int Create(Task item)
    {
        //extract the data from xml to list
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        //get the next key from config file
        int newId = Config.NextTaskId;

        //create new task
        Task t = new(newId, item.Alias, item.Description, item.CreatedAtDate, item.RequiredEffortTime, item.IsMilestone, item.Complexity, item.StartDate, item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId);

        //saving the list with new task into xml
        tasksFromXml.Add(t);
        XMLTools.SaveListToXMLSerializer<Task>(tasksFromXml, "tasks");
        return newId;
    }

    public void Delete(int id)
    {
        //extract the data from xml to list
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        List<Dependency> dependencyFromXml = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencies");
        DO.Task? task_to_del = Read(id);

        //check if the ID number received does exist & does not appear in another collection, throw ex in case not
        if (task_to_del == null) throw new DalDoesNotExistException($"Task with ID={id} not exists");
        Dependency? dep = dependencyFromXml.Find(x => x.DependensOnTask == id);
        if (dep != null) throw new DalDeletionImpossibleException("This object cannot be deleted");

        //remove the task from tasks collection & save changes in xml file
        tasksFromXml.Remove(task_to_del);
        XMLTools.SaveListToXMLSerializer<Task>(tasksFromXml, "tasks");
    }

    public Task? Read(int id)
    {
        //extract the data from xml to list
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        //return the task of received id, return null in case id doesnt exsist
        return tasksFromXml.FirstOrDefault(x => x.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        //extract the data from xml to list
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        //return the first task that meet the condition
        return tasksFromXml.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        //extract the data from xml to list
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        //return all tasks in case there is no filter
        if (filter == null)
            return tasksFromXml.Select(x => x);

        //return the tasks that meet the condition 
        return tasksFromXml.Where(filter);
    }

    public void Update(Task item)
    {
        //extract the data from xml to list
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        //check if item exsist
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} not exists");

        //delete the original task
        tasksFromXml.Remove(tasksFromXml.Find(x => x.Id == item.Id)!);

        //create the updated task & saving the list with new task into xml
        tasksFromXml.Add(item);
        XMLTools.SaveListToXMLSerializer<Task>(tasksFromXml, "tasks");
    }

    // public void Reset()
    // {
    //clear the engineers list in xml file
    //     XMLTools.SaveListToXMLSerializer<Task>(null!, "Task");
    // }
    public void Reset()
    {
        //extract the data from xml to list
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        
        //reset the list & save changes ix xml file
        tasksFromXml.Clear();
        XMLTools.SaveListToXMLSerializer<Task>(tasksFromXml, "tasks");

        //reset the task running ID to 1
        string filePath = $"{@"..\xml\" + "data-config"}.xml";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);
        XmlNode? nextTaskIdNode = xmlDoc.SelectSingleNode("/config/NextTaskId");
        if (nextTaskIdNode != null)
        {
            nextTaskIdNode.InnerText = "1"; 
        }
        xmlDoc.Save(filePath);
    }
}

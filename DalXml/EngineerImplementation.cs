namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

internal class EngineerImplementation : IEngineer
{
    //CRUD of engineer
    public int Create(Engineer newE)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        //check if the received ID doesn't exsist, throe ex in case not
        if (engineersFromXml.FirstOrDefault(item => item.Id == newE.Id) != null)
            throw new DalAlreadyExistsException($"Engineer with ID={newE.Id} already exists");

        //add the new engineer to engineers collection & update the xml file
        engineersFromXml.Add(newE);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersFromXml, "engineers");
        return newE.Id;
    }

    public void Delete(int id)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        //check if the ID number received does exist & does not appear in another collection, throw ex in case not
        if (Read(id) == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} doesn't exists");

        if (tasksFromXml.FirstOrDefault(x => x.EngineerId == id) != null)
            throw new DalDeletionImpossibleException("This object cann't be deleted");

        //delete the engineer from engineers collection & update the xml file
        engineersFromXml.Remove(engineersFromXml.Find(x => x.Id == id)!);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersFromXml, "engineers");
    }

    public Engineer? Read(int id)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        //return the engineer of received id, return null in case ID doesn't exsist
        return engineersFromXml.FirstOrDefault(x => x.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        //return the first engineer that meet the condition
        return engineersFromXml.FirstOrDefault(filter);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        //return all engineers collection in case there is no filter
        if (filter == null)
            return engineersFromXml.Select(x => x);

        //return the engineers that meet the condition 
        return engineersFromXml.Where(filter);
    }

    public void Update(Engineer newE)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        //check if item exsist
        if (Read(newE.Id) == null)
            throw new DalDoesNotExistException($"Engineer with ID={newE.Id} dosn't exists");

        //delete the original engineer
        engineersFromXml.Remove(engineersFromXml.Find(x => x.Id == newE.Id)!);

        //add the updated engineer to engineers list & update the xml file
        engineersFromXml.Add(newE);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersFromXml, "engineers");

    }

    public void Reset()
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        //reset the list & save changes ix xml file
        engineersFromXml.Clear();
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersFromXml, "engineers");
    }

}

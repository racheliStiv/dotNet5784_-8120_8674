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
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("Engineer");

        //check if the received ID doesn't exsist, throe ex in case not
        if (engineersFromXml.FirstOrDefault(item => item.Id == newE.Id) != null)
            throw new DalAlreadyExistsException($"Engineer with ID={newE.Id} already exists");

        //add the new engineer to engineers collection & update the xml page
        engineersFromXml.Add(newE);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersFromXml, "Engineer");
        return newE.Id;
    }

    public void Delete(int id)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("Engineer");
        List<Task> tasksFromXml = XMLTools.LoadListFromXMLSerializer<Task>("Task");

        //check if the ID number received does exist & does not appear in another collection, throw ex in case not
        if (Read(id) == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} doesn't exists");

        if (tasksFromXml.FirstOrDefault(x => x.EngineerId == id) != null)
            throw new DalDeletionImpossibleException("This object cann't be deleted");

        //delete the engineer from engineers collection & update the xml page
        engineersFromXml.Remove(engineersFromXml.Find(x => x.Id == id)!);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersFromXml, "Engineer");
    }

    public Engineer? Read(int id)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("Engineer");
        //return the engineer of received id, return null in case ID doesn't exsist
        return engineersFromXml.FirstOrDefault(x => x.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("Engineer");
        //return the first engineer that meet the condition
        return engineersFromXml.FirstOrDefault(filter);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("Engineer");

        //return all engineers collection in case there is no filter
        if (filter == null)
            return engineersFromXml.Select(x => x);

        //return the engineers that meet the condition 
        return engineersFromXml.Where(filter);
    }

    public void Update(Engineer newE)
    {
        //extract the data from xml to list
        List<Engineer> engineersFromXml = XMLTools.LoadListFromXMLSerializer<Engineer>("Engineer");

        //check if item exsist
        if (Read(newE.Id) == null)
            throw new DalDoesNotExistException($"Engineer with ID={newE.Id} dosn't exists");

        //delete the original engineer
        engineersFromXml.Remove(engineersFromXml.Find(x => x.Id == newE.Id)!);

        //add the updated engineer to engineers list & update the xml page
        engineersFromXml.Add(newE);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersFromXml, "Engineer");

    }

    public void Reset()
    {
        //clear the engineers list in xml page
        XMLTools.SaveListToXMLSerializer<Engineer>(null!, "Engineer");
    }

}

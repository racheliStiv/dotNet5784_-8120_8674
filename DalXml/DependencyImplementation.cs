namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    const string s_dependency = "Dependency";

    //auxiliary method to extract dependency from XElement
    static Dependency? getDependencyFromXElement(XElement? d)
    {
        //in case we got null XElement
        if (d == null)
            return null;

        //change from XElement to dependency
        return new Dependency()
        {
            Id = d.ToIntNullable("Id") ?? throw new DalDoesNotExistException($"Dependency with ID={d.ToIntNullable("Id")} not exists"),
            //error???
            DependensOnTask = d.ToIntNullable("DependensOnTask"),
            DependentTask = d.ToIntNullable("DependentTask"),
        };
    }

    public int Create(Dependency item)
    {
        //extract the data from xml file
        XElement? dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        //create XElement dependency 
        int newId = XMLTools.GetAndIncreaseNextId("Config", "dependency");
        XElement dependencyElem = new XElement(s_dependency,
            new XElement("Id", newId),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependensOnTask", item.DependensOnTask));

        //add the new one & save changes in xml file
        dependencyRootElem.Add(dependencyElem);
        XMLTools.SaveListToXMLElement(dependencyRootElem, s_dependency);
        return newId;
    }

    public void Delete(int id)
    {
        //extract the data from xml file
        XElement? dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        //check if Id exsits
        XElement? dep = (
            from d in dependencyRootElem.Elements()
            where (int?)d.Element("Id") == id
            select d).FirstOrDefault() ?? throw new DalDoesNotExistException($"Dependency with ID={id} not exists");

        //delete & update xml file
        dep.Remove();
        XMLTools.SaveListToXMLElement(dependencyRootElem, s_dependency);
    }

    public Dependency? Read(int id)
    {
        //extract the data from xml file
        XElement? dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        //return dependency after change
        return (from d in dependencyRootElem?.Elements()
                where d.ToIntNullable("Id") == id
                select getDependencyFromXElement(d)).FirstOrDefault();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        //extract the data from xml file
        XElement? dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        //return the first dependency that meet the condition
        return (from d in dependencyRootElem?.Elements()
                let dep = getDependencyFromXElement(d)
                where filter(dep)
                select dep).FirstOrDefault();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        //extract the data from xml file
        XElement? dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        //return all dependency collection after change, in case there is no filter
        if (filter == null)
            return from d in dependencyRootElem.Elements()
                   select getDependencyFromXElement(d);

        //return the dependencies after change, that meet the condition 
        return from d in dependencyRootElem.Elements()
               let dep = getDependencyFromXElement(d)
               where filter(dep)
               select dep;
    }

    // public void Reset()
    //  {
    //clear the dependencies list in xml file
    //    XMLTools.SaveListToXMLElement(null!, s_dependency);
    // }
    public void Reset()
    {
        //extract the data from xml file
        XElement? dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        //delete & update xml file
        dependencyRootElem.Remove();
        XMLTools.SaveListToXMLElement(dependencyRootElem, s_dependency);
    }


    public void Update(Dependency item)
    {
        //delete in case item exsist
        Delete(item.Id);

        //get all dependencies
        XElement? dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        //create Xelement for item
        XElement dependencyElem = new XElement(s_dependency,
            new XElement("Id", item.Id),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependensOnTask", item.DependensOnTask));

        //add the update item & save in xml file
        dependencyRootElem.Add(dependencyElem);
        XMLTools.SaveListToXMLElement(dependencyRootElem, s_dependency);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// классы для хранения угроз
namespace WpfApp1
{
  public class SecurityThreat
  {
    public int Id_UBI { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Sourse { get; set; }
    public string Target { get; set; }
    public bool IsBreachConfidentiality { get; set; }
    public bool IsBreachAccessibility { get; set; }
    public bool IsBreachIntegrity { get; set; }
    public SecurityThreat(int id, string name, string description, string sourse, string target, int conf, int acces, int integ)
    {
      Id_UBI = id;
      Name = name;
      Description = description;
      Sourse = sourse;
      Target = target;
      IsBreachConfidentiality = Convert.ToBoolean(conf);
      IsBreachAccessibility = Convert.ToBoolean(acces);
      IsBreachIntegrity = Convert.ToBoolean(integ);
    }
    public static bool operator ==(SecurityThreat t1, SecurityThreat t2)
    {
      if (t1.Id_UBI == t2.Id_UBI && t1.Name == t2.Name && t1.Description == t2.Description && t1.Sourse == t2.Sourse && t1.Target == t2.Target && t1.IsBreachConfidentiality == t2.IsBreachConfidentiality && t1.IsBreachAccessibility == t2.IsBreachAccessibility && t1.IsBreachIntegrity == t2.IsBreachIntegrity )
        return true;
      else
        return false;
    }
    public static bool operator !=(SecurityThreat t1, SecurityThreat t2)
    {
      if ((t1.Id_UBI == t2.Id_UBI) && (t1.Name != t2.Name || t1.Description != t2.Description || t1.Sourse != t2.Sourse || t1.Target != t2.Target || t1.IsBreachConfidentiality != t2.IsBreachConfidentiality || t1.IsBreachAccessibility != t2.IsBreachAccessibility || t1.IsBreachIntegrity != t2.IsBreachIntegrity))
        return true;
      else
        return false;
    }

    public override string ToString()
    {
      var sep = '~';
      return $"{Id_UBI}{sep}{Name}{sep}{Description}{sep}{Sourse}{sep}{Target}{sep}{Convert.ToInt32(IsBreachConfidentiality)}{sep}{Convert.ToInt32(IsBreachAccessibility)}{sep}{Convert.ToInt32(IsBreachIntegrity)}";
    }
  }
  public class SimpleTherat
  {
    public string Id_UBI { get; set; } = "УБИ.";
    public string Name { get; set; }
    public SimpleTherat(int id, string name)
    {
      Id_UBI += id.ToString();
      Name = name;
    }
  }
}

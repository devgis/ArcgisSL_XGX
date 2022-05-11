using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

/// <summary>
///DBService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class DBService : System.Web.Services.WebService {

    public DBService () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    private String ConnectionString = String.Empty;
    public String GetConnection()
    {
        if (String.IsNullOrEmpty(ConnectionString))
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ToString();
        }
        return ConnectionString;
    }

    [WebMethod]
    public List<GPoint> GetPoints() {
        String sql = "select p.id,p.x,p.y,p.unit,p.zone,i.pdate,i.value1,i.value2,i.value3 from t_point p left join t_pointinfo i on p.id=i.id";
        try
        {
            DataSet ds = DbHelperOra.Query(sql);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                return new List<GPoint>();
            }
            else
            {
                List<GPoint> listGPoint = new List<GPoint>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GPoint gp = new GPoint();
                    if (dr["id"] != null)
                    {
                        gp.ID = dr["id"].ToString();
                    }
                    else
                    {
                        gp.ID = string.Empty;
                    }

                    if (dr["PDATE"] != null)
                    {
                        gp.PDATE = Convert.ToDateTime(dr["PDATE"]);
                    }
                    else
                    {
                        gp.PDATE = new DateTime();
                    }

                    if (dr["UNIT"] != null)
                    {
                        gp.UNIT = dr["UNIT"].ToString();
                    }
                    else
                    {
                        gp.UNIT = String.Empty;
                    }

                    if (dr["VALUE1"] != null)
                    {
                        gp.VALUE1 = Convert.ToDecimal(dr["VALUE1"]);
                    }
                    else
                    {
                        gp.VALUE1 = Decimal.MinValue;
                    }

                    if (dr["VALUE2"] != null)
                    {
                        gp.VALUE2 = Convert.ToDecimal(dr["VALUE2"]);
                    }
                    else
                    {
                        gp.VALUE2 = Decimal.MinValue;
                    }

                    if (dr["VALUE3"] != null)
                    {
                        gp.VALUE3 = Convert.ToDecimal(dr["VALUE3"]);
                    }
                    else
                    {
                        gp.VALUE3 = Decimal.MinValue;
                    }

                    if (dr["X"] != null)
                    {
                        gp.X = Convert.ToDecimal(dr["X"]);
                    }
                    else
                    {
                        gp.X = Decimal.MinValue;
                    }

                    if (dr["Y"] != null)
                    {
                        gp.Y = Convert.ToDecimal(dr["Y"]);
                    }
                    else
                    {
                        gp.Y = Decimal.MinValue;
                    }

                    if (dr["ZONE"] != null)
                    {
                        gp.ZONE = dr["ZONE"].ToString();
                    }
                    else
                    {
                        gp.ZONE = String.Empty;
                    }
                    listGPoint.Add(gp);
                }
                return listGPoint;
            }
        }
        catch(Exception ex)
        {
            return new List<GPoint>();
        }
    }

    [WebMethod]
    public List<GResources> GetResources()
    {
        String sql = "select * from t_resources";
        try
        {
            DataSet ds = DbHelperOra.Query(sql);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                return new List<GResources>();
            }
            else
            {
                List<GResources> listGResources = new List<GResources>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GResources gr = new GResources();
                    if (dr["id"] != null)
                    {
                        gr.ID = Convert.ToInt32(dr["id"]);
                    }
                    else
                    {
                        gr.ID = -1;
                    }

                    if (dr["NAME"] != null)
                    {
                        gr.NAME = dr["NAME"].ToString();
                    }
                    else
                    {
                        gr.NAME = String.Empty;
                    }

                    if (dr["TYPE"] != null)
                    {
                        gr.TYPE = Convert.ToInt32(dr["TYPE"]);
                    }
                    else
                    {
                        gr.TYPE = -1;
                    }

                    if (dr["X"] != null)
                    {
                        gr.X = Convert.ToDecimal(dr["X"]);
                    }
                    else
                    {
                        gr.X = Decimal.MinValue;
                    }

                    if (dr["Y"] != null)
                    {
                        gr.Y = Convert.ToDecimal(dr["Y"]);
                    }
                    else
                    {
                        gr.Y = Decimal.MinValue;
                    }


                    listGResources.Add(gr);
                }
                return listGResources;
            }
        }
        catch (Exception ex)
        {
            return new List<GResources>();
        }
    }
    
}

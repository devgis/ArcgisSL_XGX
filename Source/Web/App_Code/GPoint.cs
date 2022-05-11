using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///GPoint 的摘要说明
/// </summary>
public class GPoint
{
	public GPoint()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public String ID
    {
        get;
        set;
    }

    public Decimal X
    {
        get;
        set;
    }

    public Decimal Y
    {
        get;
        set;
    }

    public String UNIT
    {
        get;
        set;
    }

    public String ZONE
    {
        get;
        set;
    }

    public DateTime PDATE
    {
        get;
        set;
    }

    public Decimal VALUE1
    {
        get;
        set;
    }

    public Decimal VALUE2
    {
        get;
        set;
    }

    public Decimal VALUE3
    {
        get;
        set;
    }
}
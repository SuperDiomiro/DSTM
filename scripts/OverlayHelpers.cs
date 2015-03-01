using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using iRTVO.Interfaces;
using iRSDKSharp;

public class Script : IScript
{
    public IHost Parent { set; get; }
    public ScriptInterfaceRequestType RequestedInterfaces { get { return ScriptInterfaceRequestType.None; } }

    static Dictionary<int, string> abbrevs = new Dictionary<int, string>();

    public String init(IHost parent)
    {
        Parent = parent;
        return "OverlayHelpers";
    }

    public String DriverInfo(String method, IStandingsItem standing, ISessionInfo session, Int32 rounding)
    {		
        switch (method.ToLower())
        {                  
            case "poschange":
                {
                    if (standing.TrackSurface == iRTVO.Interfaces.SurfaceTypes.InPitStall)
                        return "inpit";
                    int diff = standing.Position - standing.PositionLive;
                    if ( diff == 0 )
                        return "posnochange";
                    if ( diff < 0 )
                        return "posgained";
                    return "poslost";
                }
			case "startrow":
			{
                int row = (standing.PositionLive / 2) + (standing.PositionLive % 2);
				return row.ToString();
			}
            default:
                return "[invalid]";
                break;
        }
        return "[invalid]";
    }

    public String SessionInfo(String method, ISessionInfo session, Int32 rounding)
    {
        return "[invalid]";
    }

    public static string floatTime2String(Single time, Int32 showMilli, Boolean showMinutes)
    {
        bool isNegative = (time < 0.0);
        time = Math.Abs(time);

        int hours = (int)Math.Floor(time / 3600);
        int minutes = (int)Math.Floor((time - (hours * 3600)) / 60);
        Double seconds = Math.Floor(time % 60);
        Double milliseconds = Math.Round(time * 1000 % 1000, 3);
        string output;

        if (time == 0.0)
            output = "-.--";
        else if (hours > 0)
        {
            output = String.Format("{0}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        else if (minutes > 0 || showMinutes)
        {
            if (showMilli > 0)
                output = String.Format("{0}:{1:00." + "".PadLeft(showMilli, '0') + "}", minutes, seconds + milliseconds / 1000);
            else
                output = String.Format("{0}:{1:00}", minutes, seconds);
        }

        else
        {
            if (showMilli > 0)
                output = String.Format("{0:0." + "".PadLeft(showMilli, '0') + "}", seconds + milliseconds / 1000);
            else
                output = String.Format("{0}", seconds);
        }
        if (isNegative)
            return "-" + output;
        return output;
    }

    public void ButtonPress(String method)
    {
    }

    public void ApiTick(ISimulationAPI api)
    {
    }

    public void OverlayTick()
    {
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonMessage
{
    public enum Type { CREATE, MANIPULATE, CALIBRATION_DONE, DATA_TRANSMIT };

    public Type action;
    public AbsMessage message;
}

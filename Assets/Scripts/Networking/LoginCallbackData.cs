using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginCallbackData
{
    public CallbackData Data = new CallbackData(false, "");
    public UserInfo UserInfo;

    public LoginCallbackData() { }
    public LoginCallbackData(CallbackData _data, string _username, string _id)
    {
        Data = _data;
        UserInfo = new UserInfo(_id, _username);
    }
}

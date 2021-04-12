using System;
using System.Collections.Generic;

namespace Assets.Messaging
{
    public enum MessageType
    {
        RegistrationCallback,
        LoginCallback,
        InitializeGame,
        OnInvitationReceived,
        OnOpenGamePanel,
        CellClicked,
        MoveSuccess,
        MoveFailure,
        GameEnd,
        OpenGameMenu,
        GameHistoryReceived,
        OnGameHistorySelect,
        StartWatchMatch,
        OpenHistoryMenu,
        OpenRegisterMenu,
    }

    public static class MessagingSystem
    {
        private static Dictionary<MessageType, Action<object>> messages = new Dictionary<MessageType, Action<object>>();

        public static void Subscribe(MessageType _messageType, Action<object> _func)
        {
            if (!messages.ContainsKey(_messageType))
            {
                messages.Add(_messageType, _func);
            }
            else
            {
                bool exists = false;
                foreach (var action in messages[_messageType].GetInvocationList())
                {
                    if (action.Equals(_func))
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    messages[_messageType] += _func;
                }
            }
        }

        public static void Unsubscribe(MessageType _messageType, Action<object> _func)
        {
            if (messages.ContainsKey(_messageType))
            {
                messages[_messageType] -= _func;
                if (messages[_messageType] == null)
                {
                    messages.Remove(_messageType);
                }
            }
        }

        public static void DispatchMessage(MessageType _messageType, object _object)
        {
            if (messages.ContainsKey(_messageType))
            {
                messages[_messageType].Invoke(_object);
                //Debug.Log(_messageType);
            }
                
        }
        
    }
}
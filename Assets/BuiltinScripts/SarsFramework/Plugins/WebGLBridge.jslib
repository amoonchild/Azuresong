mergeInto(LibraryManager.library, 
{
    _InitNativeInfo: function(handlerObjName, handlerFuncName)
    {
    
    },

    _SendNativeData: function(msgType, msgData)
    {
        HandleUnityMessage(UTF8ToString(msgType), UTF8ToString(msgData));
    }
});
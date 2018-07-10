# <img src="resources/maqslogo.ico" height="32" width="32"> Generic Waits
MAQS includes a class of generic wait methods that can assist the webdriver with wait until a condition has been met.  This allows the webdriver added flexibility when additional handling is required.

## Overview of Wait Until and Wait For

Both types of waits will go off a configured wait time, where it will attempt or re-attempt an action every time it waits, after a configured timeout time it will either continue or throw an exception.  A method with the name WaitUntil will return a boolean value, allowing the tester to determine what, if any, action the test should take.  WaitFor will throw a webdriver timeout exception if the timeout threshold is met.

The generic waits will either go off the pre-configured wait time and timeout, or an overriden waittime and timeout.

## Wait Until

### Wait Until a Method Returns True

Waits until the method to return true.  Will return true if the method returns true, false if the method times out or returns false.
##### Written as
```csharp
bool WaitUntil(Func<Boolean>)
```
##### Example
```csharp
// A method that will return a false boolean
private bool Falsemethod()
{
    return false;
}

// The Falsemethod will always return false, as the method will always return false.  The Generic Wait will return false.
bool methodResults = GenericWait.WaitUntil(Falsemethod);

```

### Wait Until Function With Return Type T With Argument Type T Returns Type T
--------------
Waits until the method to return true, or for the method to time out. Acceps an argument of the type that is returned, this argument will be used in the passed in method.
#### Written As
```csharp
bool WaitUntil<T>(Func<T, bool> waitForTrue, T arg)
```
```csharp
// A method that will return a false boolean
private bool IsParamTestString(string testString)
{
    return testString.Equals("Test String");
}

// The IsParamTestString will always return false, as the method will always return false.  The Generic Wait will return false.
bool textResults = GenericWait.WaitUntil<string>(this.IsParamTestString, "Bad");
```
### Wait For Method with Argument to Return Type
--------------
Waits for the method to return true, or for the method to time out. Acceps an argument of the type, this argument will be used in the passed in method.
#### Written As
```csharp
void WaitFor<T>(Func<T, bool> waitForTrue, T arg)
```
#### Example
```csharp
// A method that will return a false boolean
private bool IsParamTestString(string testString)
{
    return testString.Equals("Test String");
}

// The IsParamTestString will always return false, as the method will always return false.  The generic wait will throw an exception if it times-out
bool textResults = GenericWait.WaitFor<string>(this.IsParamTestString, "Bad");
```

## Wait For

### Wait For a Method to Return True
--------------
Waits for the method to return true, does not require an argument.  If the method times-out, it will throw an exception.
##### Written As
```csharp
void WaitFor(Func<Boolean>)
```
##### Example
```csharp
// A method that will return a false boolean
private bool Falsemethod()
{
    return false;
}

// The False method will always return false.  The Generic Wait will timeout and then throw an exception.
GenericWait.WaitFor(Falsemethod);

```
The wait time and time-out can be explicitly set, as well as the ability to supress an exception being thrown.
####Written As
```csharp
void Wait(Func<bool> waitForTrue, TimeSpan retryTime, TimeSpan timeout, bool throwException)
```
#### Examples
```csharp
private bool IsFalseBool()
{
    return false;
}

// This will wait for the method IsFalseBool to return true, running and re-running the method every 100 milliseconds
// Once 500 milliseconds has passed in total, it will continue without throwing an exception
GenericWait.Wait(this.IsFalseBool, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(500), false);

// This will wait for the method IsFalseBool to return true, running and re-running the method every 100 milliseconds
// Once 500 milliseconds has passed in total, it will throw an exception
GenericWait.Wait(this.IsFalseBool, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(500), true);
```

### Wait For a Method to Return Type, And Return That Type
--------------
#### Written As
```csharp
T WaitFor<T>(Func<T> waitFor)
```
Waits for the passed in method to return true, or for the method to time out.  If the method times out, throw an exception.  The wait will return the type passed in.
#### Examples
```csharp
// A method that will return a false boolean
private bool IsParamTestString()
{
    return false;
}

// The IsParamTestString will always return false, as the method will always return false.  The generic wait will throw an exception if it times-out.
bool textResults = GenericWait.WaitFor<bool>(this.IsParamTest);
```

```csharp
// A method that will return a false boolean
private string IsParamTestString()
{
    return "stringText";
}

// The IsParamTestString will always return false, as the method will always return false.  The generic wait will throw an exception if it times-out.
string textResults = GenericWait.WaitFor<string>(this.IsParamTest);
```
The wait time and time-out can also be explicitly set.
#### Written as
```csharp
T Wait<T>(Func<T> waitFor, TimeSpan retryTime, TimeSpan timeout)
```
#### Examples
```csharp
// A method that will return a false boolean
private string IsParamTestString()
{
    return "stringText";
}

// The IsParamTestString will always return false, as the method will always return false.  The generic wait will throw an exception if it times-out.
string textResults = GenericWait.WaitFor<string>(this.IsParamTest, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(500));
```

### Wait For an Expected Argument Type, And Return a Type
--------------
#### Written As
```csharp
/// <typeparam name="T">The expected return type</typeparam>
/// <typeparam name="U">Wait for argument type</typeparam>
T WaitFor<T, U>(Func<U, T> waitFor, U arg)
```
Waits for the passed in method to return true, or for the method to time out.  If the method times out, throw an exception.  The wait will return the type passed in.
#### Examples
```csharp
private List<MailMessage> GetSearchResults(params object[] args)
{
    List<MailMessage> messageList = new List<MailMessage>();
    foreach (Lazy<MailMessage> message in this.EmailConnection.SearchMessages((SearchCondition)args[0], (bool)args[1], (bool)args[2]))
    {
        messageList.Add(message.Value);
    }

    foreach (MailMessage message in messageList)
    {
         if (message.Subject == null)
        {
            throw new Exception("Invalid results - found null subject");
        }
    }

    return messageList;
}

public virtual List<MailMessage> SearchMessages(SearchCondition condition, bool headersOnly = true, bool markRead = false)
{
    object[] args = { condition, headersOnly, markRead };
    return GenericWait.WaitFor<List<MailMessage>, object[]>(this.GetSearchResults, args);
}
```

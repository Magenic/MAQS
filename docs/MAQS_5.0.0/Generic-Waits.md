# <img src="resources/maqslogo.ico" height="32" width="32"> Generic Waits
MAQS includes a class of generic wait methods that can assist with waiting until a condition has been met. This provides additional flexibility when special handling is required.

## Overview of Wait Until and Wait For

We use waits to test if a condition was met within a certain amount of time in milliseconds, to do this we use wait time which is how long you wait between retries. These retries are bound by the timeout in milliseconds. These are set in the configuration.

We use Wait for if we want to throw an exception if the desired state is not met.  
We use Wait until if we want the boolean was met within the permitted timeout.

## Wait Until

### Wait Until a Func Returns True
This function will return true if the method returns true, false if the method times out or times out waiting for the method to return true.
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

### Wait Until Func With Return Type T With Argument Type T Returns Type T
--------------
This function will return true if the func returns true, false if the func times out or times out waiting for the func to return true.  

This wait accepts an argument that will be passed into the test method.
##### Written As
```csharp
bool WaitUntil<T>(Func<T, bool> waitForTrue, T arg)
```
##### Example

```csharp
// A method that will return a false boolean
private bool IsParamTestString(string testString)
{
    return testString.Equals("Test String");
}

// The IsParamTestString will always return false, as the method will always return false.  The Generic Wait will return false.
bool textResults = GenericWait.WaitUntil<string>(this.IsParamTestString, "Bad");
```
### Wait For Func with Argument to Return Type
--------------
This function will throw an exception if it times out before the func returns true. 

This wait accepts an argument of the type, this argument will be passed to the func.
##### Written As
```csharp
void WaitFor<T>(Func<T, bool> waitForTrue, T arg)
```
##### Example
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

### Wait For a Func to Return True
--------------
This function will throw an exception if the method times out.

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
##### Written As
```csharp
void Wait(Func<bool> waitForTrue, TimeSpan retryTime, TimeSpan timeout, bool throwException)
```
##### Examples
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

### Wait For a Func to Return Type, And Return That Type
--------------

This function will throw an exception if the method times out.

##### Written As
```csharp
T WaitFor<T>(Func<T> waitFor)
```

##### Examples
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
Waits for the passed in method to return true, or for the method to time out.  If the method times out, throw an exception.  The wait will return the type passed in.

#### Written As
```csharp
/// <typeparam name="T">The expected return type</typeparam>
/// <typeparam name="U">Wait for argument type</typeparam>
T WaitFor<T, U>(Func<U, T> waitFor, U arg)
```
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

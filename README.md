# KLog

KLog (from my last name Kaiser + the word log) is a very basic logging library which allows leveled logging (debug, info, warn, error) and supports multiple and custom writers.
By default, a new instance of the logger has a ``ConsoleWriter``. A FileWriter can be easily added.

The is an interface available called `LogWriter` which allows you to implement your own writer

## Basic Example

    var l = new Logger();
    l.Debug("Hello Debug!");
    l.Error(new Exception("ERROR!"));

This is the simplest possible use. By default, every message will be logged to console and to output. Every leveled method has two overloads,
one for strings and one for exceptions.

## Adding Writers

You can easily add any writer, e.g. a `FileWriter`:

    var l = new Logger();
    l.SetWriter(new FileWriter(@"C:\Local\debug.log"));
    l.Warn("Hello Warn!");

`SetWriter` makes sure ONLY the supplied writer is used. If you want to use multiple writer, use `AddWriter` instead.

## Custom Writers

Your custom writers must implement the `ILogWriter` interface, which looks like this:

    public interface ILogWriter
    {
        void Write(string s);
    }


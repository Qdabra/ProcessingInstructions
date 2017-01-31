# ProcessingInstructions
A small collection of .NET utilities for working with XML processing instructions

## What's this all about?
XML processing instructions often contain values encoded in what are commonly known as *pseudo-attributes*:

```xml
<?book title="Harry Potter and the Order of the Phoenix" author="J. K. Rowling" ?>
```

These look a lot like regular XML attributes, but they are not recognized by the DOM standard and most (all?) standard XML libraries provide no way of working with them.

That's where this little library comes in. It provides a few utilities for extracting these pseudo-attributes from processing instruction values and producing processing instruction values from a series of pseudo-attribute values, as well as a few other helper functions for working with processing instructions.

## Methods

### `PIHelper` class (namespace `Qdabra.Utility`)

```c#
IDictionary<string, string> GetPseudoAttributes(string processingInstructionValue)

IDictionary<string, string> GetPseudoAttributes(XProcessingInstruction processingInstruction)
```

Returns a dictionary of all of the pseudo-attributes in the provided processing instruction or processing instruction value, with the pseudo-attribute names as the keys and their values as the respective values.

<hr />

```c#
string BuildProcessingInstructionValue(IDictionary<string, string> values)
```

Creates a processing instruction value using the provided pseudo-attribute names and values.

**Throws** `FormatException` if any of the provided names is not a valid XML localname.

<hr />

```c#
XProcessingInstruction GetProcessingInstruction(XContainer parent, string name)
```

Returns the first processing instruction child of `parent` with name `name`, or `null` if no such processing instruction is found.

<hr />

### Extension methods (namespace `Qdabra.Utility.Extensions.ProcessingInstructions`)

```c#
IDictionary<string, string> GetPseudoAttributes(this XProcessingInstruction processingInstruction)
```

Convenience method for `PIHelper.GetPseudoAttributes()`

<hr />

```c#
XProcessingInstruction ProcessingInstruction(this XContainer parent, string name)
```

Convenience method for `PIHelper.GetProcessingInstruction()`

<hr />

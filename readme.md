# Redstone Scripting Language

<p align="center">
  <img src="./assert/redstone.webp" alt="Redstone Logo" width="300" />
</p>

Redstone is a custom scripting language inspired by Minecraft concepts. It reimagines common programming ideas using themed keywords while remaining a real, executable language with its own lexer, parser, AST, and runtime.

---

## Overview

The goal of Redstone is to serve both as a learning project for language design and as a practical scripting language. Familiar programming constructs are renamed to match a Minecraft-inspired theme, making the language playful while still technically grounded.

---

## Project Goals

* Explore programming language design and implementation
* Build a complete lexer-to-runtime pipeline
* Keep the language readable, themed, and easy to extend

---

## Current Features

* Numbers (integers and doubles)
* Booleans
* Variables and constants
* Function declarations
* Function calls
* Native functions

### Known Limitation

Negative numbers are not supported directly. They can be expressed using subtraction:

```
0 - 5
```

---

## Language Keywords

| Redstone Keyword | Meaning                      |
| ---------------- | ---------------------------- |
| `item`           | Variable declaration (`var`) |
| `bedrock`        | Constant declaration         |
| `workbench`      | Function declaration         |
| `air`            | `null`                       |
| `on`             | `true`                       |
| `off`            | `false`                      |

Example:

```
bedrock MAX = 10
item count = 3
```

---

## Building the Project

This project is written in C# and built using the .NET SDK.

### Prerequisites

* .NET SDK (version 7 or newer recommended)
* A command-line environment (PowerShell, Bash, etc.)

### Build Instructions

From the root of the repository:

```
dotnet build
```

This will compile the Redstone interpreter and all supporting projects.

---

## Running Redstone

### Running a Redstone Script

```
dotnet run
```

Then run the script:
```
run <path to your .rsd file>
OR
<type directly into the repl>
```

Example Redstone script:

```
workbench add(a, b) {
  a + b
}

add(2, 3)
```

---

## Native Functions

Redstone includes built-in native functions implemented in C#. These are currently focused on math utilities.

Example:

```
abs(0 - 5)
min(0,1)
max(100,10)
```

You can also print to the console with this:
```
chat("Hello World")
```

Additional native functions will be added over time.

---

## Upcoming Features

Planned improvements include:

* Native support for negative numbers
* If statements
* For loops
* While loops
* Expanded native function library

---
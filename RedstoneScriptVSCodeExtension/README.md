# Redstone VS Code Extension

This extension provides language support for the **Redstone Scripting Language**, a custom programming language inspired by Minecraft redstone concepts. It is designed to improve the development experience when writing `.rsd` files in Visual Studio Code.

---

## Features

The Redstone extension currently focuses on core language support to make editing Redstone scripts easier and safer.

### Current Features

* Syntax highlighting for Redstone keywords
* Recognition of Redstone-specific constructs:
  * `item` (variable declaration)
  * `bedrock` (constant declaration)
  * `workbench` (function declaration)
  * `air` (`null`)
  * `on` / `off` (booleans)
  * `comparator` (if statement)
* Highlighting for numbers, booleans, and function calls
* Basic structure support for `.rsd` files

---

## Requirements

There are no external dependencies required to use this extension.

To build or extend the Redstone language itself, you will need:

* .NET SDK (7 or newer recommended)

---

Enjoy writing Redstone scripts in Visual Studio Code.

# Change Log

## [0.0.1] - 1/21/26

### Added

- Initial Release

## [0.0.2] - 1/22/26

### Added

- If statements (Else, else-if, not supported yet)
  - if statements are created with the keyword: **comparator**. 
  - Example: 
    ```rsd
    comparator (on){
        chat("Hello World!")
    }
    ```

## [0.0.3] - 1/25/26

- Added Else and Else if support
  - Example:
  ```rsd
  comparator (on){
    chat("Hello World!")
  } otherwise comparator (on){
    chat("hello world!")
  } otherwise {
    chat("hello")
  }
  ```
- Added While loops
  - Example:
  ```rsd
  repeater (on) {
    chat("oh boy it's an infinite loop!)
  }
  ```
- Added break support
  - Example:
  ```rsd
  repeater (on) {
    chat("oh boy it's an infinite loop!)
    cut
  }
  ```
- Added continue support
- Example:
  ```rsd
  repeater (on) {
    chat("oh boy it's an infinite loop!)
    pulse
  }
  ```
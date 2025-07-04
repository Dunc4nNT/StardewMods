# JSON Syntax with SMAPI

<!-- TODO: more in-depth tutorial

- braces
- commas
- quotes (single/double)
- key/value
- datatypes (strings, booleans, number, null, array, object)
- newlines
- indentation
- escape character
- note on comments and trailing commas
- SMAPI tokens
- Link to more external resources
-->

```json
{
  "message.config-reloaded": "Configuration file reloaded.",
  "message.test-message": "This is a test message, this message has been shown {{count}} times."
}
```

```json
{
  "key1": "value1",
  "key2": "value2"
}
```

`key1` and `key2`, called the *keys*, are unique IDs used internally by SMAPI to display the correct phrase. You can
compare this to the unique number on your bank card. This key should be the same between languages,
so don't change it.\
`value1` and `value2`, called the *values*, are the phrases shown on your screen in-game. This is the part you'll be
changing for your translation.

There are a few characters of note here as well:

- Double quotations (`"`), these "surround" keys and values, they mark the start and end of each key and value.
If you need to include a `"` in your phrase, you should prefix it with a backslash `\`, this gives the following
result:
```json
{
  "key1": "I \"love\" Austre"
}
```
- The colon `:` between keys and values, this is used to separate keys and values.
- The comma (`,`) between the two lines (or to be more precise, between the key-value pairs).
This comma is used to say to SMAPI, "hey this is the end of the previous phrase, and there's going to be another one".
If you forget to include this comma, your file is invalid and none of your translations will work.
- The double curly braces `{{ }}` surrounding the word `count`. This is special in SMAPI, and instead of literally
showing `{{count}}`, it'll show the count, so say `42`. You don't have to worry about the internals of it, just
remember that this is a part you do not have to translate.

## Tokens

# QTemplates
This program, also known as [Q]uick Templates, is productivity tool that is used to inject text-based templates into any application to save you time. I originally started making this tool primarily for my work to save people time when doing tedious tasks and to standardize things, but then decided to evolve it on here. There are other applications like this out there, but QTemplates supports plugins for when you need to add quick access to customized tools and translated templates that are grouped together.

Releases: [https://github.com/StevenJDH/QTemplates/releases](https://github.com/StevenJDH/QTemplates/releases)

Changelog: [https://github.com/StevenJDH/QTemplates/wiki/Changelog](https://github.com/StevenJDH/QTemplates/wiki/Changelog)

## Application is still in the alpha stages with a temporary interface.
A lot of things still need to change and improve as I get a better idea of what I want to do with this application and how much time I have to work on it.

## Features
* Sits in system tray listening for hotkeys and provides quick access to tools.
* Ctrl+T opens the template selection window and Ctrl+Shift+T uses last used template.
* A plugin system to add more features in the form of tools.
* Associate templates with multiple versions written in other languages, which keeps the list clean.
* Template manager to create, edit, and delete templates.
* Filter templates by language and category.

## Plugin System
For the plugins to work, ensure that there is a folder called "Plugins" next to the QTemplates application with your plugins inside this folder. For example, place the QTemplates.Example.Plugin.dll file, which is an example plugin I made, into the Plugins folder to make it appear in the Tools menu. This example plugin just shows you how information and functionality from QTemplates can be accessed by the plugin and how QTemplates can do the same when interacting with the plugin. Keep in mind that the plugin system is still being built and will have more features when finished.

## The Database and EF6
I was going to use my own implementation for this, but decided to use an SQLite database for its portability along with Entity Framework 6x using a partial Database First Approach since currently there are some limitations with SQLite. I've only used Entity Framework Core 2x with ASP.NET and SQL, so bare with me while I iron out the details for WinForms. Below is an ERD of the database design for reference. The only thing worth pointing out is the required relationship between the Template and Version entities. The idea is that a template will always have to have at least one version, in this case I have chosen English as the default. Right now this is handled in code, but in the future I might handle this via a trigger in the database itself.

![ERD](https://github.com/StevenJDH/QTemplates/raw/master/ERD_Diagram.png "ERD Diagram")

## Do you have any questions?
Many commonly asked questions are answered in the FAQ:
[https://github.com/StevenJDH/QTemplates/wiki/FAQ](https://github.com/StevenJDH/QTemplates/wiki/FAQ)

## Need to contact me?
I can be reached here directly at [https://21.co/stevenjdh](https://21.co/stevenjdh "Contact Page")

## Want to show your support?

|Method       | Address                                                                                                    |
|------------:|:-----------------------------------------------------------------------------------------------------------|
|PayPal:      | [https://www.paypal.me/stevenjdh](https://www.paypal.me/stevenjdh "Steven's Paypal Page")                  |
|Bitcoin:     | 3GyeQvN6imXEHVcdwrZwKHLZNGdnXeDfw2                                                                         |
|Litecoin:    | MAJtR4ccdyUQtiiBpg9PwF2AZ6Xbk5ioLm                                                                         |
|Ethereum:    | 0xa62b53c1d49f9C481e20E5675fbffDab2Fcda82E                                                                 |
|Dash:        | Xw5bDL93fFNHe9FAGHV4hjoGfDpfwsqAAj                                                                         |
|Zcash:       | t1a2Kr3jFv8WksgPBcMZFwiYM8Hn5QCMAs5                                                                        |
|PIVX:        | DQq2qeny1TveZDcZFWwQVGdKchFGtzeieU                                                                         |
|Ripple:      | rLHzPsX6oXkzU2qL12kHCH8G8cnZv1rBJh<br />Destination Tag: 2357564055                                        |
|Monero:      | 4GdoN7NCTi8a5gZug7PrwZNKjvHFmKeV11L6pNJPgj5QNEHsN6eeX3D<br />&#8618;aAQFwZ1ufD4LYCZKArktt113W7QjWvQ7CWDXrwM8yCGgEdhV3Wt|


// Steven Jenkins De Haro ("StevenJDH" on GitHub)

# dotMCLauncher

Yet another Minecraft Launcher core, sucessfully migrated and slightly enhanced. Written in C#.

It makes much easier to work with Minecraft Launcher's things like:

- working with launcher profiles (`dotMCLauncher.Profiling`);
- working with version manifest and version metafiles (`dotMCLauncher.Versioning`);
- working with asset indexes (`dotMCLauncher.Resourcing`);
- working with Yggdrasil authentication requests (`dotMCLauncher.Yggdrasil`).

***Disclaimer**: I do not support making tools for any illegal distributions (games, modificaions, etc.). This core works with files from public sources and with data and files provided by users. Please, respect the job of fellows from Mojang.*

## Requirements

- Not being a dumbass;
- **.NET Framework 4**;
- At least **Visual Studio 2017**, they forced me to betray my VS2015 because of C# 7.0 features :(.

## To do

- More documentation comments;
- More NUnit tests;
- Wiki or another way to provide samples.

## Third-party tools, assemblies and code

- [JSON.NET](http://james.newtonking.com/json);
- [NUnit](https://nunit.org);
- [ReSharper](https://www.jetbrains.com/resharper/).

## License

dotMCLauncher is licensed under MIT License. See [LICENSE.md](/LICENSE.md).

```no-highlight
MIT License

Copyright (c) 2017 Igor Popov

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
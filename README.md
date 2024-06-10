
# Issuer Console App by Marcin Grzegorczyk Software

## Overview

The Issuer Console Application is a command-line tool designed to manage issues on GitHub and GitLab repositories. The application allows users to export, import, create, close, and update issues using simple commands.

## Prerequisites

- .NET SDK (version 8.0)
- A GitHub or GitLab repository with a valid authentication token

## Setup

1. Clone the repository:

```sh
git clone https://github.com/yourusername/issuer-console-app.git
cd issuer-console-app
```

2. Build the project:

```sh
dotnet build
```

## Usage

Run the application with the following command format:

```sh
dotnet run <platform> <arguments>
```

### GitHub

For GitHub, the command format is:

```sh
dotnet run github <repositoryOwner> <repositoryName> <authToken>
```

Example:

```sh
dotnet run github myUser myRepo ghp_16DigitToken
```

### GitLab

For GitLab, the command format is:

```sh
dotnet run gitlab <repositoryId> <authToken>
```

Example:

```sh
dotnet run gitlab 1234567 glpat-16DigitToken
```

## Commands

After running the application, you can enter the following commands:

### Export Issues

Export all issues to a JSON file.

```sh
export
```

You will be prompted to enter the file path where the issues will be saved.

### Import Issues

Import issues from a JSON file.

```sh
import
```

You will be prompted to enter the file path of the JSON file containing the issues.

### Create Issue

Create a new issue.

```sh
create
```

You will be prompted to enter the issue title and description.

### Close Issue

Close an existing issue by its number.

```sh
close
```

You will be prompted to enter the issue number.

### Set Issue Title

Update the title of an existing issue.

```sh
setname
```

You will be prompted to enter the issue number and the new title.

### Set Issue Description

Update the description of an existing issue.

```sh
setdesc
```

You will be prompted to enter the issue number and the new description.

### Exit

Exit the application.

```sh
exit
```

## Example Session

```sh
dotnet run github myUser myRepo ghp_16DigitToken
Enter command (export, import, create, close, setname, setdesc, exit):
create
Enter issue title:
New Issue Title
Enter issue description:
This is a description of the new issue.
Issue created successfully.
Enter command (export, import, create, close, setname, setdesc, exit):
exit
```

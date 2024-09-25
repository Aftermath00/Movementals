# Movementals

A multiplayer game with physical action to perform heroes skills

## Table of Contents

- [Introduction](#introduction)
- [Installation](#installation)
- [Usage](#usage)
- [Branching Strategy](#branching-strategy)

## Introduction

A multiplayer game with physical action to perform heroes skills

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/Aftermath00/Movementals.git
    ```
2. Navigate to the project directory:
    ```bash
    cd Movementals
    ```

## Branching Strategy

We follow the GitHub Flow branching strategy to ensure a smooth and efficient workflow. Below are the rules for creating branches:

### Branch Naming Convention

- **Fixes**: Use the `fix-[context]` naming convention for branches that address bugs or issues.
  - Example: `fix-login-bug`

- **Features**: Use the `feat-[context]` naming convention for branches that add new features or enhancements.
  - Example: `feat-user-authentication`

- **Refactoring**: Use the `refactor-[context]` naming convention for branches that involve code refactoring without changing the external behavior.
  - Example: `refactor-database-structure`

### Creating a Branch

To create a new branch, follow these steps:

1. Ensure you are on the `main` branch:
    ```bash
    git checkout main
    ```
2. Pull the latest changes:
    ```bash
    git pull origin main
    ```
3. Create and switch to a new branch:
    ```bash
    git checkout -b [branch-name]
    ```

### Committing and Pushing Changes

Commit message should be clear and descriptive!

``Avoid unclear commit message such as: "Add Feature", "Fix Bugs", etc.``

``Good commit message example: "Resolve Crash on User Login", "Optimize Database Query"``

1. Add the files you want to commit:
    ```bash
    git add [file-name]
    ```
2. Commit your changes with a meaningful message:
    ```bash
    git commit -m "Description of the changes"
    ```
3. Push the changes to the remote repository:
    ```bash
    git push origin [branch-name]
    ```

### Creating a Pull Request
1. Navigate to the repository on GitHub.
2. Click on the "Pull requests" tab.
3. Click the "New pull request" button.
4. Select the branch you want to merge into the `main` branch.
5. Add a descriptive title and comment for your pull request. Follow the title format: `[Fix/Feat/Refactor] - (Your Name) - Title`
   - Example: `[Feat] - John Doe - Add user authentication`
   - The context of the changes (what and why).
   - Any related issues (if applicable).
   - Steps to test or validate the changes.


### Code Review and Merge
1. Once the pull request is submitted, it will be reviewed by the team.
2. Address any feedback or requested changes.
3. Once approved, the pull request will be merged into the `main` branch.

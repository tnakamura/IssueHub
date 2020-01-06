# IssueHub

IssueHub is the best way to browse and maintain your GitHub issues on any iPhone, iPod Touch, and Android device!

## Getting the application to build locally

After cloning this repository to your local machine be sure create a file named `secrets.json` in the IssueHub project, and update it to look like the following:

```json
{
    "GitHubClientId": "{Your GitHub Client Id HERE}",
    "GitHubClientSecret": "{Your GitHub Client Secret HERE}",
    "GitHubAuthorizationCallbackUrl": "issuehub://oauth2redirect",
    "AndroidDataScheme": "issuehub",
    "AndroidDataPath": "/oauth2redirect"
}
```

## Contributing

Contributions are absolutely welcome!
The project is built on Xamarin.
You'll need to download this to build the project.

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D


## Credits

A log of thanks to many who contribute to open-source projects.
The following were instrumental to building this app:

- [Acr.UserDialogs](https://github.com/aritchie/userdialogs)
- [Octokit](https://github.com/octokit/octokit.net)
- [ReactiveProperty](https://github.com/runceel/ReactiveProperty)
- [ValueTaskSupplement](https://github.com/Cysharp/ValueTaskSupplement)
- [Xam.Forms.MarkdownView](https://github.com/dotnet-ad/MarkdownView)
- [Xamarin.Auth](https://github.com/xamarin/Xamarin.Auth)


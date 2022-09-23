// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.NugetNinja.Core;

public class PullRequest
{
    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }
}

public class User
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("node_id")]
    public string? NodeId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("gravatar_id")]
    public string? GravatarId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("followers_url")]
    public string? FollowersUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("following_url")]
    public string? FollowingUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("gists_url")]
    public string? GistsUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("starred_url")]
    public string? StarredUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("subscriptions_url")]
    public string? SubscriptionsUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("organizations_url")]
    public string? OrganizationsUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("repos_url")]
    public string? ReposUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("events_url")]
    public string? EventsUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("received_events_url")]
    public string? ReceivedEventsUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("site_admin")]
    public bool? SiteAdmin { get; set; }
}

public class License
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("spdx_id")]
    public string? SpdxId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("node_id")]
    public string? NodeId { get; set; }
}

public class Repository
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("node_id")]
    public string? NodeId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("private")]
    public bool? Private { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("owner")]
    public User? Owner { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("fork")]
    public bool? Fork { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("forks_url")]
    public string? ForksUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("keys_url")]
    public string? KeysUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("collaborators_url")]
    public string? CollaboratorsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("teams_url")]
    public string? TeamsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("hooks_url")]
    public string? HooksUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("issue_events_url")]
    public string? IssueEventsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("events_url")]
    public string? EventsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("assignees_url")]
    public string? AssigneesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("branches_url")]
    public string? BranchesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("tags_url")]
    public string? TagsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("blobs_url")]
    public string? BobsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("git_tags_url")]
    public string? GitTagsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("git_refs_url")]
    public string? GitRefsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trees_url")]
    public string? TreesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("statuses_url")]
    public string? StatusesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("languages_url")]
    public string? LanguagesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("stargazers_url")]
    public string? StargazersUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("contributors_url")]
    public string? ContributorsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("subscribers_url")]
    public string? SubscribersUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("subscription_url")]
    public string? SubscriptionUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("commits_url")]
    public string? CommitsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("git_commits_url")]
    public string? GitCommitsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("comments_url")]
    public string? CommentsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("issue_comment_url")]
    public string? IssueCommentUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("contents_url")]
    public string? ContentsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("compare_url")]
    public string? CompareUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("merges_url")]
    public string? MergesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("archive_url")]
    public string? ArchiveUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("downloads_url")]
    public string? DownloadsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("issues_url")]
    public string? IssuesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("pulls_url")]
    public string? PullsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("milestones_url")]
    public string? MilestonesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("notifications_url")]
    public string? NotificationsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("labels_url")]
    public string? LabelsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("releases_url")]
    public string? ReleasesUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("deployments_url")]
    public string? DeploymentsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("pushed_at")]
    public string? PushedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("git_url")]
    public string? GitUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("ssh_url")]
    public string? SshUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("clone_url")]
    public string? CloneUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("size")]
    public int Size { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("stargazers_count")]
    public int StargazersCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("watchers_count")]
    public int WatchersCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_issues")]
    public bool? HasIssues { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_projects")]
    public bool? HasProjects { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_downloads")]
    public bool? HasDownloads { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_wiki")]
    public bool? HasWiki { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_pages")]
    public bool? HasPages { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("forks_count")]
    public int ForksCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("mirror_url")]
    public string? MirrorUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("archived")]
    public bool? Archived { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("disabled")]
    public bool? Disabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("open_issues_count")]
    public int OpenIssuesCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("license")]
    public License? License { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("allow_forking")]
    public bool? AllowForking { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_template")]
    public bool? IsTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("web_commit_signoff_required")]
    public bool? WebCommitSignoffRequired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("topics")]
    public List<string> Topics { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("visibility")]
    public string? Visibility { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("forks")]
    public int Forks { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("open_issues")]
    public int OpenIssues { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("watchers")]
    public int Watchers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("default_branch")]
    public string? DefaultBranch { get; set; }

    public override string ToString()
    {
        return FullName ?? throw new NullReferenceException($"The {nameof(FullName)} of this repo is null!");
    }
}


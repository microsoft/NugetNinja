// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.NugetNinja.PrBot;

public class Owner
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
    public string? url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("node_id")]
    public string? node_id { get; set; }
}

public class Repository
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("id")]
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("node_id")]
    public string? node_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("full_name")]
    public string? full_name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("private")]
    public bool? Private { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("owner")]
    public Owner owner { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("html_url")]
    public string? html_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("description")]
    public string? description { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("fork")]
    public bool? fork { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("url")]
    public string? url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("forks_url")]
    public string? forks_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("keys_url")]
    public string? keys_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("collaborators_url")]
    public string? collaborators_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("teams_url")]
    public string? teams_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("hooks_url")]
    public string? hooks_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("issue_events_url")]
    public string? issue_events_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("events_url")]
    public string? events_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("assignees_url")]
    public string? assignees_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("branches_url")]
    public string? branches_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("tags_url")]
    public string? tags_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("blobs_url")]
    public string? blobs_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("git_tags_url")]
    public string? git_tags_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("git_refs_url")]
    public string? git_refs_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trees_url")]
    public string? trees_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("statuses_url")]
    public string? statuses_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("languages_url")]
    public string? languages_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("stargazers_url")]
    public string? stargazers_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("contributors_url")]
    public string? contributors_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("subscribers_url")]
    public string? subscribers_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("subscription_url")]
    public string? subscription_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("commits_url")]
    public string? commits_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("git_commits_url")]
    public string? git_commits_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("comments_url")]
    public string? comments_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("issue_comment_url")]
    public string? issue_comment_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("contents_url")]
    public string? contents_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("compare_url")]
    public string? compare_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("merges_url")]
    public string? merges_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("archive_url")]
    public string? archive_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("downloads_url")]
    public string? downloads_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("issues_url")]
    public string? issues_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("pulls_url")]
    public string? pulls_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("milestones_url")]
    public string? milestones_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("notifications_url")]
    public string? notifications_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("labels_url")]
    public string? labels_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("releases_url")]
    public string? releases_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("deployments_url")]
    public string? deployments_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? created_at { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("updated_at")]
    public string? updated_at { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("pushed_at")]
    public string? pushed_at { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("git_url")]
    public string? git_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("ssh_url")]
    public string? ssh_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("clone_url")]
    public string? CloneUrl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("svn_url")]
    public string? svn_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("homepage")]
    public string? homepage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("size")]
    public int size { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("stargazers_count")]
    public int stargazers_count { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("watchers_count")]
    public int watchers_count { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("language")]
    public string? language { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_issues")]
    public bool? HasIssues { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_projects")]
    public bool? has_projects { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_downloads")]
    public bool? has_downloads { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_wiki")]
    public bool? has_wiki { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_pages")]
    public bool? has_pages { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("forks_count")]
    public int forks_count { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("mirror_url")]
    public string? mirror_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("archived")]
    public bool? archived { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("disabled")]
    public bool? disabled { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("open_issues_count")]
    public int Open_issues_count { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("license")]
    public License License { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("allow_forking")]
    public bool? allow_forking { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_template")]
    public bool? is_template { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("web_commit_signoff_required")]
    public bool? web_commit_signoff_required { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("topics")]
    public List<string> Topics { get; set; }
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
}


variable "project_id" {
  type        = string
  description = "The GCP project ID"
  default     = "otto-fixme"
}

variable "region" {
  type        = string
  description = "The GCP region"
  default     = "us-central1"
}

variable "github_repository" {
  type        = string
  description = "The GitHub repository in the format owner/repo (e.g., 'octocat/Hello-World')"
  default     = "ottolote/fixme"
}

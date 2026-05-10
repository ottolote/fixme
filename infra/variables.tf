variable "project_id" {
  type        = string
  description = "The GCP project ID"
  default     = "otto-fixme"
}

variable "region" {
  type        = string
  description = "The GCP region"
  default     = "europe-north1"
}

variable "zone" {
  type        = string
  description = "The GCP zone for the low-cost GKE cluster"
  default     = "europe-north1-a"
}

variable "cluster_name" {
  type        = string
  description = "The GKE cluster name"
  default     = "fixme"
}

variable "github_repository" {
  type        = string
  description = "The GitHub repository in the format owner/repo (e.g., 'octocat/Hello-World')"
  default     = "ottolote/fixme"
}

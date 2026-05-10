# ==============================================================================
# Service Account for GitHub Actions
# ==============================================================================
resource "google_service_account" "github_actions" {
  account_id   = "github-actions-sa"
  display_name = "GitHub Actions Service Account"
  description  = "Service account used by GitHub Actions CI/CD to apply Terraform"
}

# Grant necessary permissions to the GitHub Actions SA
# We grant roles/editor here for basic IaC provisioning, but you should lock this down
# to the specific roles needed for your resources later.
resource "google_project_iam_member" "github_actions_editor" {
  project = var.project_id
  role    = "roles/editor"
  member  = "serviceAccount:${google_service_account.github_actions.email}"
}

# ==============================================================================
# Workload Identity Federation (WIF) for GitHub Actions
# This allows GitHub Actions to authenticate to GCP without long-lived JSON keys.
# ==============================================================================
resource "google_iam_workload_identity_pool" "github_pool" {
  workload_identity_pool_id = "github-actions-pool"
  display_name              = "GitHub Actions Pool"
  description               = "Identity pool for GitHub Actions integrations"
}

resource "google_iam_workload_identity_pool_provider" "github_provider" {
  workload_identity_pool_id          = google_iam_workload_identity_pool.github_pool.workload_identity_pool_id
  workload_identity_pool_provider_id = "github-actions-provider"
  display_name                       = "GitHub Actions Provider"

  attribute_mapping = {
    "google.subject"       = "assertion.sub"
    "attribute.actor"      = "assertion.actor"
    "attribute.repository" = "assertion.repository"
    "attribute.aud"        = "assertion.aud"
  }

  attribute_condition = "assertion.repository == \"${var.github_repository}\""


  oidc {
    issuer_uri = "https://token.actions.githubusercontent.com"
  }
}

# Bind the WIF identity to the GitHub Actions Service Account
resource "google_service_account_iam_member" "github_actions_wif_binding" {
  service_account_id = google_service_account.github_actions.name
  role               = "roles/iam.workloadIdentityUser"
  member             = "principalSet://iam.googleapis.com/${google_iam_workload_identity_pool.github_pool.name}/attribute.repository/${var.github_repository}"
}

# ==============================================================================
# Service Account for OpenCode Agent
# ==============================================================================
resource "google_service_account" "opencode" {
  account_id   = "opencode-agent-sa"
  display_name = "OpenCode Agent Service Account"
  description  = "Minimal read-only service account for OpenCode"
}

# Grant read-only permissions to the OpenCode SA
resource "google_project_iam_member" "opencode_viewer" {
  project = var.project_id
  role    = "roles/viewer"
  member  = "serviceAccount:${google_service_account.opencode.email}"
}

# ==============================================================================
# Outputs
# ==============================================================================
output "github_actions_service_account_email" {
  value       = google_service_account.github_actions.email
  description = "The email of the GitHub Actions SA"
}

output "workload_identity_provider_name" {
  value       = google_iam_workload_identity_pool_provider.github_provider.name
  description = "The name of the Workload Identity Provider to use in GitHub Actions"
}

output "opencode_service_account_email" {
  value       = google_service_account.opencode.email
  description = "The email of the OpenCode SA"
}

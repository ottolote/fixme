data "google_project" "current" {
  project_id = var.project_id
}

resource "google_artifact_registry_repository" "containers" {
  location      = var.region
  repository_id = var.artifact_registry_repository
  description   = "Container images for FixMe services"
  format        = "DOCKER"

  depends_on = [google_project_service.artifactregistry]
}

resource "google_artifact_registry_repository_iam_member" "gke_nodes_reader" {
  project    = var.project_id
  location   = google_artifact_registry_repository.containers.location
  repository = google_artifact_registry_repository.containers.repository_id
  role       = "roles/artifactregistry.reader"
  member     = "serviceAccount:${data.google_project.current.number}-compute@developer.gserviceaccount.com"
}

output "artifact_registry_repository" {
  value       = google_artifact_registry_repository.containers.repository_id
  description = "The Artifact Registry repository for container images"
}

output "fixme_api_image_repository" {
  value       = "${var.region}-docker.pkg.dev/${var.project_id}/${google_artifact_registry_repository.containers.repository_id}/fixme-api"
  description = "The FixMe API image repository URL"
}

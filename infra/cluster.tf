# ==============================================================================
# Minimal Low-Cost Kubernetes Cluster
# ==============================================================================
resource "google_container_cluster" "primary" {
  name     = var.cluster_name
  location = var.zone

  deletion_protection      = false
  initial_node_count       = 1
  remove_default_node_pool = true

  release_channel {
    channel = "REGULAR"
  }

  depends_on = [
    google_project_service.compute,
    google_project_service.container,
  ]
}

resource "google_container_node_pool" "primary_spot" {
  name       = "spot-pool"
  location   = google_container_cluster.primary.location
  cluster    = google_container_cluster.primary.name
  node_count = 1

  management {
    auto_repair  = true
    auto_upgrade = true
  }

  node_config {
    machine_type = "e2-small"
    disk_size_gb = 30
    disk_type    = "pd-standard"
    spot         = true

    oauth_scopes = [
      "https://www.googleapis.com/auth/cloud-platform",
    ]

    labels = {
      workload = "fixme"
      cost     = "spot"
    }
  }
}

output "gke_cluster_name" {
  value       = google_container_cluster.primary.name
  description = "The GKE cluster name"
}

output "gke_cluster_location" {
  value       = google_container_cluster.primary.location
  description = "The GKE cluster location"
}

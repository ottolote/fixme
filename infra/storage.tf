# ==============================================================================
# Terraform State Storage
# ==============================================================================
resource "google_storage_bucket" "tf_state" {
  name          = "${var.project_id}-tfstate"
  location      = "US"
  force_destroy = false

  uniform_bucket_level_access = true

  versioning {
    enabled = true
  }
}

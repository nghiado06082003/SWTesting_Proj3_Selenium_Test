mod test_suites;
use std::error::Error;
use Selenium_Test_Rust::{driver_port::get_driver_port, caps::get_caps};
use crate::test_suites::test_gpa_calculator::test_gpa_calculator;
use thirtyfour::prelude::*;

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error + Send + Sync>> {
    let caps = get_caps();
    let port = get_driver_port();
    let driver = WebDriver::new(format!("http://localhost:{port}"), caps).await?;
    
    test_gpa_calculator(&driver).await?;

    driver.quit().await?;

    Ok(())
}

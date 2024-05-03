mod test_helpers;

use std::{env, error::Error, path::Path};
use futures::StreamExt;
use test_helpers::read_test_suites;
use Selenium_Test_Rust::{driver_port::get_driver_port, caps::get_caps};
use thirtyfour::prelude::*;

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error + Send + Sync>> {
    let caps = get_caps();
    let port = get_driver_port();
    let driver = WebDriver::new(format!("http://localhost:{port}"), caps).await?;
    test_runner(&driver).await;
    driver.quit().await?;

    Ok(())
}

async fn test_runner(web_driver: &WebDriver) {
    let test_root_dir = String::from(Path::new(&env::current_dir().unwrap()).join("src").join("bin").join("test_level_1").join("test_suites").to_str().unwrap());
    let mut testcase_stream = read_test_suites(test_root_dir).boxed();
    
    while let Some(testcase) = testcase_stream.next().await {
        println!("Running testcase {testcase:?}");
    }
}

mod test_helpers;

use std::{env, error::Error, path::Path, time::Duration};
use futures::StreamExt;
use test_helpers::{read_test_suites, Output, TestCase};
use Selenium_Test_Rust::{caps::get_caps, driver_port::get_driver_port, gpa_site::{change_course_name, change_credits, change_grade, click_calculate, get_error_messages, get_result, visit_and_reset}};
use thirtyfour::prelude::*;

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error + Send + Sync>> {
    let caps = get_caps();
    let port = get_driver_port();
    let driver = WebDriver::new(format!("http://localhost:{port}"), caps).await?;
    test_runner(&driver).await?;
    driver.quit().await?;

    Ok(())
}

async fn test_runner(web_driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    let test_root_dir = String::from(Path::new(&env::current_dir().unwrap()).join("src").join("bin").join("test_level_1").join("test_suites").to_str().unwrap());
    let mut testcase_stream = read_test_suites(test_root_dir).boxed();
    
    while let Some(testcase) = testcase_stream.next().await {
        println!("Running testcase {testcase:?}");
        run_test_case(web_driver, testcase).await?;
        println!("Test passed!");
    }
    Ok(())
}

async fn run_test_case(web_driver: &WebDriver, testcase: TestCase) -> Result<(), Box<dyn Error + Send + Sync>> {
    visit_and_reset(web_driver).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_course_name(web_driver, 1, &testcase.input.course_name).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_credits(web_driver, 1, testcase.input.credits).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_grade(web_driver, 1, {
        let grade_json = serde_json::to_string(&testcase.input.grade).unwrap();
        String::from(&grade_json[1..grade_json.len() - 1])
    }.as_str()).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    click_calculate(web_driver).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    match testcase.output {
        Output::Ok(expected_gpa) => {
            let gpa = get_result(web_driver).await?; 
            assert_eq!(gpa, expected_gpa);
        }
        Output::Error(expected_errors) => {
            let errors = get_error_messages(web_driver).await?;
            assert_eq!(errors, expected_errors);
        }
    }
    tokio::time::sleep(Duration::new(1, 0)).await;
    Ok(())
}

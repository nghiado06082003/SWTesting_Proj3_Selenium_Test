mod test_helpers;

use std::{env, error::Error, path::Path, time::Duration};
use futures::StreamExt;
use test_helpers::{read_test_suites, Output, TestCase};
use Selenium_Test_Rust::{caps::get_caps, driver_port::get_driver_port, gpa_site::{change_course_name, change_credits, change_grade, click_calculate, get_error_messages, get_result, get_result_when_zero_credit, visit_and_reset}};
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
    println!("Test equivalent class!");
    let test_equivalent_class_root_dir = String::from(Path::new(&env::current_dir().unwrap()).join("src").join("bin").join("test_level_1").join("test_suites").join("equivalent_class").to_str().unwrap());
    run_test_strategy(web_driver, test_equivalent_class_root_dir).await?;
    println!("Done testing equivalent class!");

    println!("Test boundary value!");
    let test_boundary_value_root_dir = String::from(Path::new(&env::current_dir().unwrap()).join("src").join("bin").join("test_level_1").join("test_suites").join("boundary_value_analysis").to_str().unwrap());
    run_test_strategy(web_driver, test_boundary_value_root_dir).await?;
    println!("Done testing boundary value!");

    println!("Test use case!");
    let test_use_case_root_dir = String::from(Path::new(&env::current_dir().unwrap()).join("src").join("bin").join("test_level_1").join("test_suites").join("use_case").to_str().unwrap());
    run_test_strategy(web_driver, test_use_case_root_dir).await?;
    println!("Done testing use case!");

    Ok(())
}

async fn run_test_strategy(web_driver: &WebDriver, test_root_dir: String) -> Result<(), Box<dyn Error + Send + Sync>> {
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
        Output::ZeroCredit(expected_res) => {
            let res = get_result_when_zero_credit(web_driver).await?;
            assert_eq!(res, expected_res);
        }
    }
    tokio::time::sleep(Duration::new(1, 0)).await;
    Ok(())
}

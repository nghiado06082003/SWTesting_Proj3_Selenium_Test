use core::time::Duration;
use thirtyfour::prelude::*;
use std::error::Error;
use Selenium_Test_Rust::gpa_site::*;

pub async fn test_gpa_calculator(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test GPA calculator suite is running");
    test1(driver).await?;
    test2(driver).await?;
    test3(driver).await?;
    test4(driver).await?;
    println!("Test GPA calculator suite runs successfully");
    Ok(())
}

pub async fn test1(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    visit_and_reset(driver).await?;
    println!("Test1 is running!");
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_course_name(driver, 1, "Math").await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_credits(driver, 1, -1).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_grade(driver, 1, "C").await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    click_calculate(driver).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    let errors = get_error_messages(driver).await?;
    assert_eq!(errors, vec!["Please provide a valid credit for item #1."]);
    println!("Test1 passed!");
    tokio::time::sleep(Duration::new(1, 0)).await;
    Ok(())
}

pub async fn test2(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    visit_and_reset(driver).await?;
    println!("Test2 is running!");
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_course_name(driver, 1, "Math").await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_credits(driver, 1, 1).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_grade(driver, 1, "B").await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    click_calculate(driver).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    let gpa = get_result(driver).await?; 
    assert_eq!(gpa, 3);
    println!("Test2 passed!");
    tokio::time::sleep(Duration::new(1, 0)).await;
    Ok(())
}

pub async fn test3(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    visit_and_reset(driver).await?;
    println!("Test3 is running!");
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_course_name(driver, 1, "Math").await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_credits(driver, 1, -1).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_grade(driver, 1, "-").await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    click_calculate(driver).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    let errors = get_error_messages(driver).await?;
    assert_eq!(errors, vec!["Please provide a valid credit for item #1.", "Please provide a valid grade for item #1."]);
    change_course_name(driver, 1, "Math").await?;
    println!("Test3 passed!");
    tokio::time::sleep(Duration::new(1, 0)).await;
    Ok(())
}

pub async fn test4(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    visit_and_reset(driver).await?;
    println!("Test4 is running!");
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_course_name(driver, 1, "Math").await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_credits(driver, 1, 4).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    change_grade(driver, 1, "-").await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    click_calculate(driver).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    let errors = get_error_messages(driver).await?;
    assert_eq!(errors, vec!["Please provide a valid grade for item #1."]);
    change_course_name(driver, 1, "Math").await?;
    println!("Test4 passed!");
    tokio::time::sleep(Duration::new(1, 0)).await;
    Ok(())
}


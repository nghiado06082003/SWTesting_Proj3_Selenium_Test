use thirtyfour::prelude::*;
use std::error::Error;
use Selenium_Test_Rust::gpa_site::*;

pub async fn test_gpa_calculator(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test GPA calculator suite is running");
    test1(driver).await?;
    test2(driver).await?;
    test3(driver).await?;
    test4(driver).await?;
    Ok(())
}

pub async fn test1(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test1 is running!");
    visit_and_reset(driver).await?;
    change_course_name(driver, 1, "Math").await?;
    change_credits(driver, 1, -1).await?;
    change_grade(driver, 1, "C").await?;
    click_calculate(driver).await?;
    let errors = get_error_messages(driver).await?;
    assert_eq!(errors, vec!["Please provide a valid credit for item #1."]);
    println!("Test1 passed!");
    Ok(())
}

pub async fn test2(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test2 is running!");
    visit_and_reset(driver).await?;
    Ok(())
}

pub async fn test3(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test3 is running!");
    visit_and_reset(driver).await?;
    Ok(())
}

pub async fn test4(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test4 is running!");
    visit_and_reset(driver).await?;
    Ok(())
}


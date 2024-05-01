use thirtyfour::prelude::*;
use std::error::Error;

static URL: &'static str = "https://www.calculator.net/gpa-calculator.html";

pub async fn test_GPA_calculator(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test GPA calculator suite is running");
    test1(driver).await?;
    test2(driver).await?;
    test3(driver).await?;
    test4(driver).await?;
    Ok(())
}

pub async fn test1(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test1 is running!");
    driver.goto(URL).await?;
    Ok(())
}

pub async fn test2(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test2 is running!");
    driver.goto(URL).await?;
    Ok(())
}

pub async fn test3(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test3 is running!");
    driver.goto(URL).await?;
    Ok(())
}

pub async fn test4(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    println!("Test4 is running!");
    driver.goto(URL).await?;
    Ok(())
}


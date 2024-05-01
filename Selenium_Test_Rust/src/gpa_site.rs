use thirtyfour::prelude::*;
use std::error::Error;

static URL: &'static str = "https://www.calculator.net/gpa-calculator.html";

pub async fn change_course_name(driver: &WebDriver, index: usize, name: &str) -> Result<(), Box<dyn Error + Send + Sync>> {
    let course_name_text_box = driver.find(By::Css(format!("input[type='text'][name='da{index}']"))).await?;
    course_name_text_box.clear().await?;
    course_name_text_box.send_keys(name).await?;
    Ok(())
}

pub async fn change_credits(driver: &WebDriver, index: usize, credits: isize) -> Result<(), Box<dyn Error + Send + Sync>> {
    let credits_text_box = driver.find(By::Css(format!("input[type='text'][name='ca{index}']"))).await?;
    credits_text_box.clear().await?;
    credits_text_box.send_keys(format!("{credits}")).await?;
    Ok(())
}

pub async fn change_grade(driver: &WebDriver, index: usize, grade: &str) -> Result<(), Box<dyn Error + Send + Sync>> {
    let grade_option = if grade != "-" {
        driver.find(By::Css(format!("select[name='la{index}'] > option[value='{}']", grade.to_lowercase()))).await?
    } else {
        driver.find(By::Css(format!("select[name='la{index}'] > option"))).await?
    };
    grade_option.click().await?;
    Ok(())
}

pub async fn visit_and_reset(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    driver.goto(URL).await?;
    let clear_button = driver.find(By::Css("input[type='button'][value='Clear']")).await?;
    clear_button.click().await?;
    for i in 1..=3 {
        change_grade(driver, i, "-").await?;            
    }
    Ok(())
}

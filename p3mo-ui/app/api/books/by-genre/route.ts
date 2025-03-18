import { NextRequest, NextResponse } from 'next/server';

const API_URL = process.env.API_URL || 'http://localhost:5078/api';

export async function GET() {
  try {
    const response = await fetch(`${API_URL}/books/bygenre`);
    
    if (!response.ok) {
      throw new Error(`Error fetching books by genre: ${response.statusText}`);
    }
    
    const data = await response.json();
    return NextResponse.json(data);
  } catch (error) {
    console.error('Error in books by genre API route:', error);
    return NextResponse.json(
      { error: 'Failed to fetch books by genre' },
      { status: 500 }
    );
  }
}